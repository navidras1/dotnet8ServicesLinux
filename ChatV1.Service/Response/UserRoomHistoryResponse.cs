using ChatV1.DataAccess.CommonModels;
using ChatV1.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class UserRoomHistoryResponse:ResponseMessage
    {
        public List<ResponseV2_V2> roomHistories { get; set; }
        //public List<ChatLog> roomHistories { get; set; }
    }

    public class UserRoomHistoryResponseV2 : ResponseMessage
    {
        public Dictionary<string, HistoryMessage> Result { get; set; } = new();
    }

    public class RoomHistory
    {
        public string fromUserName { get; set; }
        //public int countOfUnreadMessage { get; set; }
        public string chatRoomName { get; set; }
        public string lastMessage { get; set; }
        public long createDate { get; set; }
        public int pkEmployee { get; set; }
        public string fullName { get; set; }
        public bool? isRtl { get; set; }
        public string status { get; set; }
        public Guid messageId { get; set; }
        public GetFileDetailResponse attachment { get; set; } = null;

        public RoomHistory GetObjFromChatLog(ChatLog x, string chatRoomName , Dictionary<int, string> chatStatusDict)
        {
            RoomHistory roomHistory = new() { createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], chatRoomName = chatRoomName };
            return roomHistory;

        }

        public ResponseV2_V2 GetObjFromChatLogWithRoomType(ChatLog x, string chatRoomName ,string chatRoomType  , Dictionary<int, string> chatStatusDict)
        {
            //RoomHistory roomHistory = new() { createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], chatRoomName = chatRoomName };
            ResponseV2_V2 responseV2_V2 = new() { createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], chatRoomName = chatRoomName , chatRoomType = chatRoomType, replyOf = x.Reply, forwardedBy= x.ForwardedBy };
            return responseV2_V2;

        }


    }
}
