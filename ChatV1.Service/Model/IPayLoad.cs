using ChatV1.DataAccess.Context;
using ChatV1.DataAccess.Models;
using ChatV1.DataAccess.Repository;
using ChatV1.Service.Model.PayloadParams;
using ChatV1.Service.Request;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Algorithm.Locate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model
{
    public interface IPayLoad
    {
        public void Perform(string parameter);
    }

    public class SEEN_MESSAGE : IPayLoad
    {
        private readonly IChatV1Repository<ChatLog> _chatLog;
        private readonly IChatV1Repository<ChatStatus> _chatStatus;
        private readonly ILogger<SEEN_MESSAGE> _logger;
        private readonly ChatV1Context _context;

        public SEEN_MESSAGE(IChatV1Repository<ChatLog> chatLog, IChatV1Repository<ChatStatus> chatStatus, ILogger<SEEN_MESSAGE> logger, ChatV1Context context)
        {
            _chatLog = chatLog;
            _logger = logger;
            _context = context;
            _chatStatus = chatStatus;
        }

        public void Perform(string parameter)
        {
            var general = JsonConvert.DeserializeObject<GeneralPayLoad>(parameter);
            var payloadStr = JsonConvert.SerializeObject(general.payLoad);


            try
            {
                var inputParam = JsonConvert.DeserializeObject<SeenMessageParam>(payloadStr);
                var foundChatLog = _chatLog.Find(x => x.ChatGuid == inputParam.messageGuid && x.FromUserName == inputParam.toUserName && x.ToUserName == inputParam.fromUserName).FirstOrDefault();
                if (foundChatLog != null)
                {
                    var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.ChatStatus1, x => x.Id);
                    var listOfBefore = _chatLog.Find(x => x.ClientDateTime <= foundChatLog.ClientDateTime && x.FromUserName == inputParam.toUserName && x.ToUserName == inputParam.fromUserName && x.ChatStatusId == chatStatusDict["sent"]).ToList();

                    listOfBefore.ForEach(x => x.ChatStatusId = chatStatusDict["seen"]);
                    _context.BulkUpdate(listOfBefore);
                }
                else
                {
                    _logger.LogError($"{parameter} not found");
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }

            //throw new NotImplementedException();
        }
    }

    public class DELETE_MESSAGE : IPayLoad
    {
        private readonly IChatV1Repository<ChatLog> _chatLog;
        private readonly IChatV1Repository<ChatStatus> _chatStatus;
        private readonly ILogger<SEEN_MESSAGE> _logger;
        private readonly ChatV1Context _context;

        public DELETE_MESSAGE(IChatV1Repository<ChatLog> chatLog, IChatV1Repository<ChatStatus> chatStatus, ILogger<SEEN_MESSAGE> logger, ChatV1Context context)
        {
            _chatLog = chatLog;
            _chatStatus = chatStatus;
            _logger = logger;
            _context = context;
        }


        public void Perform(string parameter)
        {
            var general = JsonConvert.DeserializeObject<GeneralPayLoad>(parameter);
            var payloadStr = JsonConvert.SerializeObject(general.payLoad);

            try
            {
                var inputParam = JsonConvert.DeserializeObject<DeleteMessageParam>(payloadStr);
                var foundChat = _chatLog.Find(x => x.FromUserName == inputParam.fromUserName && x.ToUserName == inputParam.toUserName && inputParam.messageGuid.Contains(x.ChatGuid)).ToList();
                foundChat.ForEach(x => x.IsDeleted = true);
                _context.BulkUpdate(foundChat);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }


    public class DELETE_MESSAGE_ROOM : IPayLoad
    {
        private readonly IChatV1Repository<ChatLog> _chatLog;
        private readonly IChatV1Repository<ChatStatus> _chatStatus;
        private readonly IChatV1Repository<DataAccess.Models.ChatRoom> _chatRoom;
        private readonly ILogger<SEEN_MESSAGE> _logger;
        private readonly ChatV1Context _context;

        public DELETE_MESSAGE_ROOM(IChatV1Repository<ChatLog> chatLog, IChatV1Repository<ChatStatus> chatStatus, ILogger<SEEN_MESSAGE> logger, ChatV1Context context,  IChatV1Repository<DataAccess.Models.ChatRoom> chatRoom)
        {
            _chatLog = chatLog;
            _chatStatus = chatStatus;
            _logger = logger;
            _context = context;
            _chatRoom= chatRoom;
        }


        public void Perform(string parameter)
        {
            var general = JsonConvert.DeserializeObject<GeneralPayLoad>(parameter);
            var payloadStr = JsonConvert.SerializeObject(general.payLoad);

            try
            {
                var inputParam = JsonConvert.DeserializeObject<DeleteMessageRoomParam>(payloadStr);
                var check = CheckIfUserCanDeleteChatLog(new CheckIfUserCanDeleteChatLogRequest { MessageGuids = inputParam.ChatLogGuids, RoomName = inputParam.RoomName, UserName = inputParam.FromUserName });
                if (check != null && check.Count!=0)
                {
                    var foundChatLogs = _chatLog.Find(x => check.Keys.Contains(x.Id)).ToList();
                    foundChatLogs.ForEach(chatLog => chatLog.IsDeleted = true);
                    _context.BulkUpdate(foundChatLogs);

                    List<OfflineAction> offlineActionList = new();

                    foreach (var i in inputParam.UserList)
                    {

                        var action = new { type = "DELETE_MESSAGE_ROOM", payLoad = new { fromUserName = inputParam.FromUserName, roomName = inputParam.RoomName, messageGuids = check.Values.ToList() } };
                        var theAction = JsonConvert.SerializeObject(action);
                        OfflineAction offlineAction = new()
                        {
                            ActionTypeId = 3,
                            CreateDateTime = DateTime.Now,
                            Done = false,
                            FromUseName = inputParam.FromUserName,
                            ToUserName = i,
                            DoneDateTime = null,
                            Action = theAction
                        };
                        offlineActionList.Add(offlineAction);
                    }

                    _context.BulkInsert(offlineActionList);
                }

                //var foundChat = _chatLog.Find(x => x.FromUserName == inputParam.fromUserName && x.ToUserName == inputParam.toUserName && inputParam.messageGuid.Contains(x.ChatGuid)).ToList();
                //foundChat.ForEach(x => x.IsDeleted = true);
                //_context.BulkUpdate(foundChat);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private Dictionary<long, Guid> CheckIfUserCanDeleteChatLog(CheckIfUserCanDeleteChatLogRequest request)
        {
            Dictionary<long, Guid> res = new();

            var chatRoom = _chatRoom.Find(x => x.ChatRoomName == request.RoomName).FirstOrDefault();
            if (chatRoom == null)
            {
                //res.IsSuccess = false;
                //res.Message = "Chat room not found";
                return null;
            }

            //res = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == chatRoom.Id).Where(x => request.MessageGuids.Contains(x.chatLog.ChatGuid)).Select(x => x.chatLog.Id).ToList();
            res = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == chatRoom.Id).Where(x => request.MessageGuids.Contains(x.chatLog.ChatGuid) && x.chatLog.FromUserName==request.UserName).ToDictionary(x => x.chatLog.Id, y => y.chatLog.ChatGuid);

            //var foundLogs = _chatLog.Find(x => x.FromUserName == request.UserName && request.MessageGuids.Contains(x.ChatGuid)).Include(x=> x.ChatRoomLogs.Where(y=> y.ChatRoomId==foundChatRoom.Id)).ToList();
            return res;
        }
    }

}
