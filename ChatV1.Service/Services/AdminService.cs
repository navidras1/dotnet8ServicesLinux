using ChatV1.DataAccess.CommonModels;
using ChatV1.DataAccess.Context;
using ChatV1.DataAccess.Models;
using ChatV1.DataAccess.Repository;
using ChatV1.Service.Model.Admin;
using ChatV1.Service.Model.ChatRoom;
using ChatV1.Service.Request;
using ChatV1.Service.Response;
using ChatV1.Service.Services.CallApi;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services
{
    public interface IAdminService
    {
        ChatRoomAddMembersResponse ChatRoomAddMembers(ChatRoomAddMembersRequest request);
        CreateChatRoomResponse? CreateChatRoom(CreateChatRoomRequest request);
        CreateRoomTypeResponse CreateRoomType(CreateChatRoomTypeRequest request);
        Task<ResponseMessage> CreateNoticeChannelAndSupportGroup(CreateNoticeChannelAndSupportGroupRequest request);
        bool IsSuperUser(string userName);
        CanSendMessageToRoomResponse CanSendMessageToRoom(IsUserRoomAdminRequest request);
        public CanSendMessageToRoomResponseV2 CanSendMessageToRoomV2(IsUserRoomAdminRequestV2 request);
        public CheckIfUserIsRoomMemberResponse CheckIfUserIsRoomMember(string userName, string chatRoomName);
        public ResponseMessage AddUserFromGriffin(AddUserFromGriffinRequest request);
        public CheckIfUserIsRoomMemberResponse CheckIfUserIsRoomMemberV2(string userName, long chatRoomId);
    }

    public class AdminService : IAdminService
    {
        private ILogger<AdminService> _logger;
        private IChatV1Repository<ChatRoom> _chatRoom;
        private IChatV1Repository<ChatRoomMemeber> _chatRoomMemeber;
        private IChatV1Repository<ChatRoomType> _chatRoomType;
        private readonly IGriffinAirAvation _griffinAirAvation;
        private readonly IChatV1Repository<EmpMaster> _empMaster;
        private readonly ChatV1Context _context;
        private readonly IChatV1Repository<ChatLog> _chatLog;


        public AdminService(ILogger<AdminService> logger, IChatV1Repository<ChatRoom> chatRoom, IChatV1Repository<ChatRoomMemeber> chatRoomMemeber, IChatV1Repository<ChatRoomType> chatRoomType, IGriffinAirAvation griffinAirAvation, IChatV1Repository<EmpMaster> empMaster, ChatV1Context context, IChatV1Repository<ChatLog> chatLog)
        {
            _logger = logger;
            _chatRoom = chatRoom;
            _chatRoomMemeber = chatRoomMemeber;
            _chatRoomType = chatRoomType;
            _griffinAirAvation = griffinAirAvation;
            _empMaster = empMaster;
            _context = context;
            _chatLog = chatLog;
        }

        public CreateRoomTypeResponse CreateRoomType(CreateChatRoomTypeRequest request)
        {
            CreateRoomTypeResponse response = new();
            try
            {
                var foundRoomType = _chatRoomType.Find(x => x.Name == request.Name).FirstOrDefault();
                if (foundRoomType != null)
                {
                    response.Id = foundRoomType.Id;
                    response.Name = foundRoomType.Name;
                    response.Warnings.Add("Room Exists");
                }
                else
                {
                    ChatRoomType chatRoomType = new()
                    {
                        IsChannel = request.IsChannel,
                        Istemp = request.Istemp,
                        MaxMember = request.MaxMember,
                        Name = request.Name,
                        Description = request.Description,
                    };
                    var res = _chatRoomType.Add(chatRoomType);
                    response.Id = res.Id;
                    response.Name = response.Name;

                }

            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public CreateChatRoomResponse? CreateChatRoom(CreateChatRoomRequest request)
        {
            CreateChatRoomResponse response = new();

            try
            {
                var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();
                if (foundChatRoom != null)
                {
                    response.Id = foundChatRoom.Id;
                    response.ChatRoomName = foundChatRoom.ChatRoomName;
                    response.Warnings.Add("Room already exist");

                }
                else
                {
                    ChatRoom chatRoom = new()
                    {
                        ChatRoomName = request.ChatRoomName,
                        ChatRoomTypeId = request.ChatRoomTypeId,
                        CreateDatetime = DateTime.UtcNow,
                        IsActive = true,
                        CreatorUserName = request.CreatorUserName,
                        Description = request.Description,
                    };
                    var res = _chatRoom.Add(chatRoom);
                    response.ChatRoomName = res.ChatRoomName;
                    response.Id = res.Id;
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ChatRoomAddMembersResponse ChatRoomAddMembers(ChatRoomAddMembersRequest request)
        {
            ChatRoomAddMembersResponse response = new();

            List<ChatRoomMemeber> chatRoomMemebers = new();

            try
            {
                var reqMembers = request.ChatRoomMembers.Select(member => member.UserName);
                var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();
                var foundMemberTest = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && reqMembers.Contains(x.UserName));
                if (foundMemberTest.Count() != reqMembers.Count())
                {
                    //var foundMemberTest1 = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && !reqMembers.Contains(x.UserName)).ToList();
                    var mmm = reqMembers.Except(foundMemberTest.Select(x => x.UserName)).ToList();
                    var chatRoomMembersNotInList =  request.ChatRoomMembers.Where(x=> mmm.Contains(x.UserName)).ToList();

                    if (foundChatRoom != null)
                    {
                        foreach (var i in chatRoomMembersNotInList)
                        {
                            
                                ChatRoomMemeber chatRoomMemeber = new()
                                {
                                    IsActive = true,
                                    IsAdmin = i.IsAdmin,
                                    UserName = i.UserName,
                                    CreateDateTime = DateTime.UtcNow,
                                    ChatRoomId = foundChatRoom.Id,
                                    UserId = 0

                                };
                                //var addedMember = _chatRoomMemeber.Add(chatRoomMemeber);
                                chatRoomMemebers.Add(chatRoomMemeber);

                            
                        }
                        _context.BulkInsert(chatRoomMemebers);
                        //_context.BulkRead()
                        //var addMemberRangeRes =  _chatRoomMemeber.AddRange(chatRoomMemebers);
                        foreach (var i in chatRoomMemebers)
                        {
                            response.AddMembers.Add(new AddMemberResult { MemberId = i.Id, Name = i.UserName });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseMessage> CreateNoticeChannelAndSupportGroup(CreateNoticeChannelAndSupportGroupRequest request)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                CreateRoomTypeResponse groupRoomTypeRes = CreateRoomType(new CreateChatRoomTypeRequest { IsChannel = false, MaxMember = 10000, Istemp = false, Description = "", Name = "Group" });
                var griffin_SupportRoomRes = CreateChatRoom(new CreateChatRoomRequest { ChatRoomTypeId = groupRoomTypeRes.Id, ChatRoomName = "Griffin_Support", CreatorUserName = request.CreatorUserName });

                //var usersToChat = await _griffinAirAvation.GetAllEmployeeForChat(new Request.GetUsersToChatRequest { notUsersIn = "", pageNumber = 1, pageSize = 3000, toUserName = "", userName = "" });
                var usersToChat = await _griffinAirAvation.GetAllEmployeesForChannelChat();

                List<string> admins = ["100737", "Shahriyari", "s.khosravi", "hamed"];


                ChatRoomAddMembersRequest chatRoomAddMembersRequestGriffinSupport = new();
                chatRoomAddMembersRequestGriffinSupport.ChatRoomName = "Griffin_Support";

                var users = usersToChat.result;
                foreach (var i in users)
                {
                    var userName = i["UserName"].ToString().ToLower();
                    bool isAdmin = false;
                    if (admins.Contains(userName))
                    {
                        isAdmin = true;
                    }
                    chatRoomAddMembersRequestGriffinSupport.ChatRoomMembers.Add(new ChatRoomMemberModel { IsAdmin = isAdmin, UserName = userName });

                }

                var griffinSupportCreateResult = ChatRoomAddMembers(chatRoomAddMembersRequestGriffinSupport);
                if (griffinSupportCreateResult.Warnings.Count > 0)
                {
                    foreach (var i in griffinSupportCreateResult.Warnings)
                    {
                        responseMessage.Warnings.Add(i);
                    }
                }

                //////////////////////////////////////////////////////////////////

                groupRoomTypeRes = CreateRoomType(new CreateChatRoomTypeRequest { IsChannel = true, MaxMember = 10000, Istemp = false, Description = "", Name = "channel" });
                var griffin_NoticeRes = CreateChatRoom(new CreateChatRoomRequest { ChatRoomTypeId = groupRoomTypeRes.Id, ChatRoomName = "Griffin_Notice", CreatorUserName = request.CreatorUserName });

                ChatRoomAddMembersRequest chatRoomAddMembersRequestGriffinNotice = new();
                chatRoomAddMembersRequestGriffinNotice.ChatRoomName = "Griffin_Notice";


                foreach (var i in users)
                {
                    var userName = i["UserName"].ToString().ToLower();
                    bool isAdmin = false;
                    if (admins.Contains(userName))
                    {
                        isAdmin = true;
                    }
                    chatRoomAddMembersRequestGriffinNotice.ChatRoomMembers.Add(new ChatRoomMemberModel { IsAdmin = isAdmin, UserName = userName });

                }

                var griffinNoticeCreateResult = ChatRoomAddMembers(chatRoomAddMembersRequestGriffinNotice);
                if (griffinSupportCreateResult.Warnings.Count > 0)
                {
                    foreach (var i in griffinNoticeCreateResult.Warnings)
                    {
                        responseMessage.Warnings.Add(i);
                    }
                }


            }
            catch (Exception ex)
            {

                responseMessage.IsSuccess = false;
                responseMessage.Message = ex.Message;
            }
            return responseMessage;
            //CreateChatRoom(new CreateChatRoomRequest { })
        }

        public bool IsSuperUser(string userName)
        {
            var res = false;
            var foundEmpMaster = _empMaster.Find(x => x.UserName == userName && (x.IsSuperUser == true)).FirstOrDefault();
            if (foundEmpMaster != null)
            {
                res = true;
            }

            return res;
        }

        public IsUserRoomAdminResponse IsUserRoomAdmin(IsUserRoomAdminRequest request)
        {
            IsUserRoomAdminResponse response = new IsUserRoomAdminResponse();
            try
            {
                var foundRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();
                if (foundRoom == null)
                {
                    response.IsAdmin = false;
                    response.Message = "Room not found";
                    return response;
                }
                var foundMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundRoom.Id && x.UserName == request.userName && x.IsAdmin == true).FirstOrDefault();
                if (foundMember == null)
                {
                    response.IsAdmin = false;
                    response.Message = "Member not found of not admin";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsAdmin = false;
                response.Message = "Member not found of not admin";
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }

            return response;

        }

        public IsUserRoomAdminResponse IsUserRoomAdminV2(IsUserRoomAdminRequestV2 request)
        {
            IsUserRoomAdminResponse response = new IsUserRoomAdminResponse();
            try
            {
                var foundRoom = _chatRoom.Find(x => x.Id == request.ChatRoomId).FirstOrDefault();
                if (foundRoom == null)
                {
                    response.IsAdmin = false;
                    response.Message = "Room not found";
                    return response;
                }
                var foundMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundRoom.Id && x.UserName == request.userName && x.IsAdmin == true).FirstOrDefault();
                if (foundMember == null)
                {
                    response.IsAdmin = false;
                    response.Message = "Member not found of not admin";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsAdmin = false;
                response.Message = "Member not found of not admin";
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }

            return response;

        }

        public IsRoomChannelResponse IsRoomChannel(string roomName)
        {
            IsRoomChannelResponse response = new();

            var foundRoom = _chatRoom.Find(x => x.ChatRoomName == roomName).FirstOrDefault();
            if (foundRoom == null)
            {
                response.IsRoomChannel = false;
                response.Message = "Room not found";
                return response;
            }

            var foundType = _chatRoomType.Find(x => x.Id == foundRoom.ChatRoomTypeId && x.IsChannel == true).FirstOrDefault();
            if (foundType == null)
            {
                response.IsRoomChannel = false;
            }
            return response;

        }

        public IsRoomChannelResponse IsRoomChannelV2(long roomId)
        {
            IsRoomChannelResponse response = new();

            var foundRoom = _chatRoom.Find(x => x.Id == roomId).FirstOrDefault();
            if (foundRoom == null)
            {
                response.IsRoomChannel = false;
                response.Message = "Room not found";
                return response;
            }

            var foundType = _chatRoomType.Find(x => x.Id == foundRoom.ChatRoomTypeId && x.IsChannel == true).FirstOrDefault();
            if (foundType == null)
            {
                response.IsRoomChannel = false;
            }
            return response;

        }

        public CanSendMessageToRoomResponse CanSendMessageToRoom(IsUserRoomAdminRequest request)
        {
            CanSendMessageToRoomResponse response = new();

            var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();

            if (foundChatRoom == null)
            {
                response.CanSend = false;
                response.Message = "Room not found";
                return response;
            }
            else
            {
                if (foundChatRoom.IsActive != true)
                {
                    response.CanSend = false;
                    response.Message = "Room is not active";
                    return response;
                }
            }

            var foundMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName == request.userName).FirstOrDefault();
            if (foundMember == null)
            {
                response.CanSend = false;
                response.Message = $"User {request.userName} is not member of {request.ChatRoomName}";
                return response;
            }

            var isAdminRes = IsUserRoomAdmin(request);
            var isChannel = IsRoomChannel(request.ChatRoomName);
            response.RoomType = isChannel.IsRoomChannel == true ? "channel" : "group";
            if (isChannel.IsRoomChannel)
            {
                if (isAdminRes.IsAdmin)
                {
                    response.CanSend = true;
                }
                else
                {
                    response.CanSend = false;
                    response.Message = "not Admin";
                }
            }
            else
            {
                response.CanSend = true;

            }
            return response;
        }

        public CanSendMessageToRoomResponseV2 CanSendMessageToRoomV2(IsUserRoomAdminRequestV2 request)
        {
            CanSendMessageToRoomResponseV2 response = new();

            var foundChatRoom = _chatRoom.Find(x => x.Id == request.ChatRoomId).FirstOrDefault();

            if (foundChatRoom == null)
            {
                response.CanSend = false;
                response.Message = "Room not found";
                return response;
            }
            else
            {
                if (foundChatRoom.IsActive != true)
                {
                    response.CanSend = false;
                    response.Message = "Room is not active";
                    return response;
                }
            }

            var foundMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName == request.userName).FirstOrDefault();
            if (foundMember == null)
            {
                response.CanSend = false;
                response.Message = $"User {request.userName} is not member of {request.ChatRoomId}";
                return response;
            }

            var isAdminRes = IsUserRoomAdminV2(request);
            var isChannel = IsRoomChannelV2(request.ChatRoomId);
            response.RoomType = isChannel.IsRoomChannel == true ? "channel" : "group";
            if (isChannel.IsRoomChannel)
            {
                if (isAdminRes.IsAdmin)
                {
                    response.CanSend = true;
                }
                else
                {
                    response.CanSend = false;
                    response.Message = "not Admin";
                }
            }
            else
            {
                response.CanSend = true;
                response.CanGetPushNotification = foundChatRoom.CanGetPushNotification??false;

            }
            return response;
        }

        public CheckIfRoomExistsResponse CheckIfRoomExists(string roomName)
        {
            CheckIfRoomExistsResponse response = new();

            try
            {
                var founChatRoom = _chatRoom.Find(x => x.ChatRoomName == roomName).FirstOrDefault();
                if (founChatRoom == null)
                {
                    response.RoomFound = false;
                    response.Message = "Room not found";
                }
                else
                {
                    response.RoomId = founChatRoom.Id;
                }
            }
            catch (Exception ex)
            {

                response.RoomFound = false;
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public CheckIfRoomExistsResponse CheckIfRoomExistsV2(long roomId)
        {
            CheckIfRoomExistsResponse response = new();

            try
            {
                var founChatRoom = _chatRoom.Find(x => x.Id == roomId).FirstOrDefault();
                if (founChatRoom == null)
                {
                    response.RoomFound = false;
                    response.Message = "Room not found";
                }
                else
                {
                    response.RoomId = founChatRoom.Id;
                }
            }
            catch (Exception ex)
            {

                response.RoomFound = false;
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public CheckIfUserIsRoomMemberResponse CheckIfUserIsRoomMember(string userName, string chatRoomName)
        {
            CheckIfUserIsRoomMemberResponse response = new();


            var checkChatRoomResult = CheckIfRoomExists(chatRoomName);
            if (checkChatRoomResult.RoomFound != true)
            {
                response.IsRoomMember = false;
                response.Message = checkChatRoomResult.Message;
                return response;
            }

            var foundMember = _chatRoomMemeber.Find(x => x.UserName == userName && x.ChatRoomId == checkChatRoomResult.RoomId).FirstOrDefault();
            if (foundMember == null)
            {
                response.IsRoomMember = false;
                response.Message = "Member not Found";
                return response;
            }
            response.RoomId = checkChatRoomResult?.RoomId;

            return response;
        }

        public CheckIfUserIsRoomMemberResponse CheckIfUserIsRoomMemberV2(string userName, long chatRoomId)
        {
            CheckIfUserIsRoomMemberResponse response = new();


            var checkChatRoomResult = CheckIfRoomExistsV2(chatRoomId);
            if (checkChatRoomResult.RoomFound != true)
            {
                response.IsRoomMember = false;
                response.Message = checkChatRoomResult.Message;
                return response;
            }

            var foundMember = _chatRoomMemeber.Find(x => x.UserName == userName && x.ChatRoomId == checkChatRoomResult.RoomId).FirstOrDefault();
            if (foundMember == null)
            {
                response.IsRoomMember = false;
                response.Message = "Member not Found";
                return response;
            }
            response.RoomId = checkChatRoomResult?.RoomId;

            return response;
        }

        public ResponseMessage AddUserFromGriffin(AddUserFromGriffinRequest request)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var foundUsers = _chatLog.Find(x => x.FromUserName == request.UserName || x.ToUserName == request.UserName).ToList();
                foreach (var user in foundUsers)
                {
                    user.IsDeleted = false;
                }

                var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == "Griffin_Notice").FirstOrDefault();
                if (foundChatRoom != null)
                {
                    var foundRoomMember = _chatRoomMemeber.Find(x => x.UserName == request.UserName && x.ChatRoomId == foundChatRoom.Id).FirstOrDefault();

                    if (foundRoomMember == null)
                    {
                        ChatRoomMemeber chatRoomMemeber = new()
                        {
                            ChatRoomId = foundChatRoom.Id,
                            CreateDateTime = DateTime.Now,
                            IsActive = true,
                            IsAdmin = false,
                            UserName = request.UserName,
                            UserId=0
                        };
                        _chatRoomMemeber.Add(chatRoomMemeber);
                    }
                }
                _chatLog.UpdateRange(foundUsers);
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;




        }

        public ResponseMessage DeleteUserFromGriffin(AddUserFromGriffinRequest request)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var foundUsers = _chatLog.Find(x => x.FromUserName == request.UserName || x.ToUserName == request.UserName).ToList();
                foreach (var user in foundUsers)
                {
                    user.IsDeleted = true;
                }

                var foundChatRoom = _chatRoom.Find(x => x.CreatorUserName == "Griffin_Notice").FirstOrDefault();
                if (foundChatRoom != null)
                {
                    var foundRoomMember = _chatRoomMemeber.Find(x => x.UserName == request.UserName && x.ChatRoomId == foundChatRoom.Id).FirstOrDefault();

                    if (foundRoomMember != null)
                    {
                        
                        _chatRoomMemeber.Remove(foundRoomMember);
                    }
                }
                _chatLog.UpdateRange(foundUsers);
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;

        }
    }
}
