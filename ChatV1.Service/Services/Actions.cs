using Azure;
using Azure.Core;
using ChatV1.DataAccess.CommonModels;
using ChatV1.DataAccess.Context;
using ChatV1.DataAccess.Models;
using ChatV1.DataAccess.Repository;
using ChatV1.Service.Model;
using ChatV1.Service.Model.ChatRoom;
using ChatV1.Service.Request;
using ChatV1.Service.Response;
using ChatV1.Service.Response.GetAllEmployeeForChat;
using ChatV1.Service.Services.CallApi;
using ChatV1.Service.Services.SocketIo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Triangulate;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static ChatV1.Service.Response.RoomMembersResponse;
using EFCore.BulkExtensions;
using System.Security.Cryptography;
using Minio;
using ChatV1.Service.Services.Minio;
using ChatV1.Service.Response.PrivateChatSocketIO;
using ChatV1.Service.Response.GetEployeeDetails;
using System.Net.WebSockets;



namespace ChatV1.Service.Services
{
    public interface IActions
    {
        List<ChatLog> GetChatHistory(GetChatHistoryRequest request);
        public Task LogTheChat(LogTheChatRequest request);
        public List<GetUnreadMessagesResponse> GetUnreadMessages(GetUnreadMessagesRequest request);
        public UpdateUserToReadMessagResponse UpdateUserToReadMessag(UpdateUserToReadMessagRequest request);
        public GetLastMessageResponse GetLastMessage(GetLastMessageRequest request);
        public Task<HistoryMessageOfUsersResponse> HistoryMessageOfUsers(HistoryMessageOfUsersRequest request);

        public void LogTheChatRoom(LogTheChatRoomRequest request);
        public GetUserRoomsResponse GetUserRooms(GetUserRoomsRequest request);
        public List<GetUsersChatRoomsWithCountOfUnreadsResponse> GetUsersChatRoomsWithCountOfUnreads(string UserName);
        public GetAllFromSPWithOutputResponseViewModel GetUsersChatroomMessages(GetUsersChatroomMessagesRequest request);
        public Task<AddToContactListResponse> AddToContactList(AddToContactListRequest request, bool checkUser = true);
        public RemoveFromContactsResponse RemoveFromContacts(RemoveFromContactsRequest request);
        public Task<GetContactListResponse> GetContactList(GetContactListRequest request);
        public GetAllFromSPWithOutputResponseViewModel GetCountAndLastMessagePrivateMessage(GetCountAndLastMessagePrivateMessageRequest request);

        public Task<GetAllEmployeeForChatResponse?> GetAllEmployeeForChat(GetUsersToChatRequest request);
        public List<string?> GetMastersOfContact(GetMastersOfContactRequest request);
        public ResponseMessage SetLastEmpLastSeen(SetLastEmpLastSeenRequest request);
        public GetLastSeenOfEmpResponse GetLastSeenOfEmp(GetLastSeenOfEmpRequest request);
        public Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryV2(GetCountAndLastMessagePrivateMessageRequest request);
        public Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryUnreadMessage(GetUserChatHistoyUreadMessageRequest request);
        public CountOfUnreadMessageResponse CountOfUnreadMessage(string fromUserName, string toUserName);
        public Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryV3(GetCountAndLastMessagePrivateMessageRequest request);
        public GetAllFromSPWithOutputResponseViewModel GetCountAndLastMessagePrivateMessageV2(GetCountAndLastMessagePrivateMessageRequest request);
        public Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryUnreadMessageV2(GetUserChatHistoyUreadMessageRequest request);
        public PrivateChatUploadFileResponse PrivateChatUploadFile(PrivateChatUploadFileRequest request);
        public GetAttachmentDetailsResponse GetAttachmentDetails(DownloadAttachmentRequest request);
        public Task<GetAllEmployeeForChatResponse?> GetAllEmployeeForChat(GetAllEmployeeForChatRequest request);
        public List<GetUserRoomsResponse2> GetUserRoomsV2(GetUserRoomsRequest request);
        public Task SaveChatRoomMessage(ChatRoomMessage chatRoomMessage);
        public UsersChatRoomsWithCountOfUnreadsResponseV2 GetUsersChatRoomsWithCountOfUnreadsV2(ListOfRoomsWithUnreadMessagesRequestV2 request);
        public Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryV3_V2(GetCountAndLastMessagePrivateMessageRequestV2 request);
        public Task<HistoryMessageOfUsersResponseV2> HistoryMessageOfUsersV2(HistoryMessageOfUsersRequestV2 request);
        public Task<HistoryChatOfUsersResponse> HistoryChatOfUsers(string toUserName, Dictionary<string, Guid?> request);
        public Task<PrivateChatResponse> PrivateChatService(PrivateChatMessageRequest request);
        public Task<GetAttachmentDetailsResponse> GetAttachmentDetailsTest(DownloadAttachmentRequestTest request);
        public Task<GetAttachmentDetailsResponseFile> GetAttachmentDetailsTestFile(DownloadAttachmentRequestTest request);
        public GetFileDetailResponse GetFileDetail(GetFileDetailRequest request);
        public UpdateChatRoomMessagesToReadResponse UpdateChatRoomMessagesToRead(UpdateChatRoomMessagesToReadRequest request);
        public UsersChatRoomsWithCountOfUnreadsResponseV3 GetUsersChatRoomsWithCountOfUnreadsV3(ListOfRoomsWithUnreadMessagesRequestV3 request);
        public Task<UserRoomHistoryResponse> UserRoomHistory(UserRoomHistoryRequest request);
        public Task<HistoryChatOfUsersResponse> HistoryChatOfUsersV2(HistoryChatOfUsersV2Request request);
        public Task<RoomMembersResponse> RoomMembers(RoomMembersRequest request);
        public DeletePrivateChatResponse DeletePrivateChat(DeletePrivateChatServiceRequest request);
        public CreateRoomResponse CreateChannelGroupRoom(CreateRoomRequest request);
        public Task<GetAllEmployeeForChatResponse?> GetAllEmployeeForChatV2(GetAllEmployeeForChatRequest request);

        public AddUsersToRoomResponse DeleteUserFromRoom(DeleteUserFromRoomRequest request);
        public AddUsersToRoomResponse MakeUsersAdminOrNot(MakeUsersAdminOrNotRequest request);
        public CheckOfflineActionsResponse CheckOfflineActions(string userName);
        public ResponseMessage InstertToOffLineAction(InstertToOffLineActionRequest request);
        public ResponseMessage UpdateOfflineActionByType(UpdateOfflineActionByTypeRequest request);

        public ResponseMessage UpdateMessageToReadFromTo(UpdateMessageToReadFromToRequest request);
        public ResponseMessage DeleteChatOffline(DeleteChatOfflineRequest request);
        public ResponseMessage MakeOfflineActionDone(MakeOfflineActionDoneRequest request);
        public Task<PoolingResponse> Pooling(GetCountAndLastMessagePrivateMessageRequestV2 request);
        public Task<SendMessaageToRoomResponse> SendMessageToFlighChannel(SendMessageToFlighChannelRequest request);
        public Task<UserRoomHistoryResponse> UserRoomHistoryV2(UserRoomHistoryRequestV3 request);
        public Task<RoomMembersResponse> RoomMembersWithNotif(RoomMembersRequest request);
        public Task<RoomMembersResponse> RoomMembersWithNotifV2(RoomMembersRequestV2 request);
        public ResponseMessage SetUserRoomPushNotification(SetUserRoomPushNotificationRequest request);
        public GetUserChatRoomNotificationStatusResponse GetUserChatRoomNotificationStatus(GetUserChatRoomNotificationStatusRequest request);
        public Task<PrivateChatSocketIOResponse> SendMessaageToRoom(SendMessaageToRoomRequest request);
        public Task<PrivateChatResponse> PrivateChatBulkMessages(PrivateChatMessageRequest request);
        public Task<PrivateChatResponse> PrivateChatToUsersService(PrivateChatMessageToUsersRequest request);
        public DisableRoomResponse DisableRoom(DisableRoomRequest request);
        public CheckUserRolesResponse CheckUserRoles(CheckUserRolesRequest request);
        public Task<PoolingV2Response> PoolingV2(PoolingV2Request request);
    }

    public class Actions : IActions
    {
        private readonly IChatV1Repository<ChatLog> _chatLog;
        private readonly IChatV1Repository<ChatStatus> _chatStatus;
        private readonly IChatV1Repository<ChatRoomType> _chatRoomType;
        private readonly IChatV1Repository<ChatRoom> _chatRoom;
        private readonly IChatV1Repository<ChatRoomMemeber> _chatRoomMemeber;
        private readonly IChatV1Repository<ChatRoomLog> _chatRoomLog;
        private readonly IChatV1Repository<EmpMaster> _empMaster;
        private readonly IChatV1Repository<UserContanct> _userContanct;
        private readonly IChatV1Repository<UserChatRoomReciever> _userChatRoomReciever;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGriffinAirAvation _griffinAirAvation;
        private readonly IChatV1Repository<ChatAttachment> _chatAttachment;
        private readonly IChatV1Repository<ChatLogAttachment> _chatLogAttachment;
        private readonly ILogger<Actions> _logger;
        private IHostEnvironment _hostingEnvironment;
        private readonly ChatV1Context _context;
        private readonly IChat _chat;
        private readonly IAdminService _adminService;
        private readonly IChatV1Repository<OfflineAction> _offLineAction;
        private readonly IChatV1Repository<ActionType> _actionType;
        private readonly IServiceProvider _provider;
        private IMinIOService _minIOService;
        private IRobotChat _robotChat;
        //private readonly IMinioClient _minioClient;


        public Actions(IChatV1Repository<ChatLog> chatLog,
            IChatV1Repository<ChatStatus> chatStatus,
            IChatV1Repository<ChatRoomType> chatRoomType,
            IChatV1Repository<ChatRoom> chatRoom,
            IChatV1Repository<ChatRoomMemeber> chatRoomMemeber,
            IChatV1Repository<ChatRoomLog> chatRoomLog,
            IChatV1Repository<EmpMaster> empMaster,
            IChatV1Repository<UserContanct> userContanct,
            IChatV1Repository<UserChatRoomReciever> userChatRoomReciever,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IGriffinAirAvation griffinAirAvation,
            IChatV1Repository<ChatLogAttachment> chatLogAttachment,
            IChatV1Repository<ChatAttachment> chatAttachment,
            ILogger<Actions> logger,
            //IHostEnvironment hostEnvironment,
            ChatV1Context context,
            IChat chat,
            IAdminService adminService,
            IChatV1Repository<OfflineAction> offLineAction,
            IChatV1Repository<ActionType> actionType,
            IServiceProvider provider,
            [FromKeyedServices("ChatMinIO")] IMinIOService minIOService
            , IRobotChat robotChat
            )
        {
            _chatLog = chatLog;
            _chatStatus = chatStatus;
            _chatRoomType = chatRoomType;
            _chatRoom = chatRoom;
            _chatRoomMemeber = chatRoomMemeber;
            _chatRoomLog = chatRoomLog;
            _empMaster = empMaster;
            _userContanct = userContanct;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _griffinAirAvation = griffinAirAvation;
            _chatLogAttachment = chatLogAttachment;
            _chatAttachment = chatAttachment;
            _logger = logger;
            //_hostingEnvironment = hostEnvironment;
            _context = context;
            _userChatRoomReciever = userChatRoomReciever;
            _chat = chat;
            _adminService = adminService;
            this._offLineAction = offLineAction;
            this._actionType = actionType;
            _provider = provider;
            //_minioClient = minioClient;
            _minIOService = minIOService;
            _robotChat = robotChat;
        }

        public async Task LogTheChat(LogTheChatRequest request)
        {

            if (request.ReplyOfGuid == "null")
            {
                request.ReplyOfGuid = null;
            }

            ChatResponse chatResponse = new ChatResponse();
            long theAttachmentId = request.AttachmentId ?? 0;
            if (request.AttachmentId != 0 && request.AttachmentId != null)
            {
                var foundAttachment = _chatAttachment.GetById(request.AttachmentId);
                if (foundAttachment == null)
                {
                    chatResponse.Success = false;
                    chatResponse.Message = "Attachment file not found please upload file";
                    _logger.LogError("Attachment file not found please upload file");
                    return;
                }
            }

            var chatCreatedTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(request.CreateDate)).UtcDateTime;

            var clientDateTime = DateTimeOffset.FromUnixTimeMilliseconds(request.ClientDateTime.Value).UtcDateTime;


            try
            {

                // var fromUserNameDetails = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.FromUserName });
                // var toUserNameDetails = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.ToUserName });
                foreach (var message in request.Messages)
                {
                    if (string.IsNullOrEmpty(message.Message))
                    {
                        message.Message = " ";
                    }

                    ChatLog chatLog = new ChatLog
                    {
                        ChatGuid = request.ChatGuid,
                        ChatStatusId = request.ChatStatusId,
                        CreateDate = DateTimeOffset.Now,

                        ClientDateTime = clientDateTime,

                        //FromEmpId = request.FromEmpId,
                        FromEmpId = Convert.ToInt32(1),

                        //ToEmPid = request.ToEmPid,
                        ToEmPid = Convert.ToInt32(1),

                        Message = message.Message,
                        FromUserName = request.FromUserName,
                        ToUserName = request.ToUserName,
                        IsRtl = message.IsRtl,
                        ForwardedBy = request.ForwardedBy,
                        Reply = request.ReplyOfGuid

                    };

                    //    FnSpRequest fnSpRequest = new FnSpRequest();
                    //    fnSpRequest.FNSpName = "GSP_InsertLogToPrivateChat";
                    //    fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
                    //    new ServiceOperatorParameter(){Name="Message" , Value=request.Message },
                    //    new ServiceOperatorParameter(){Name="ChatStatusId ", Value=request.ChatStatusId },
                    //    new ServiceOperatorParameter(){Name="CreateDate" , Value= chatCreatedTime},
                    //    new ServiceOperatorParameter(){Name="ChatGuid" , Value= request.ChatGuid},
                    //    new ServiceOperatorParameter(){Name="FromUserName" , Value= request.FromUserName},
                    //    new ServiceOperatorParameter(){Name="ToUserName" , Value= request.ToUserName},
                    //    new ServiceOperatorParameter(){Name="FromEmpId" , Value=fromUserNameDetails.result[0]["pkEmployee"]},
                    //    new ServiceOperatorParameter(){Name="ToEmPId" , Value=toUserNameDetails.result[0]["pkEmployee"]}

                    //});
                    //    var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);

                    var insertedCahtLog = _chatLog.Add(chatLog);
                    _chatRoomLog.Add(new ChatRoomLog { ChatLogId = insertedCahtLog.Id, ChatRoomId = 12 });
                    if (theAttachmentId != 0)
                    {
                        ChatLogAttachment chatLogAttachment = new ChatLogAttachment { ChatAttachmentId = theAttachmentId, ChatLogId = insertedCahtLog.Id };
                        _chatLogAttachment.Add(chatLogAttachment);
                    }
                }

                AddToContactList(new AddToContactListRequest() { OwnerUserName = request.ToUserName, Usernames = new List<string> { request.FromUserName } }, false);
                AddToContactList(new AddToContactListRequest() { OwnerUserName = request.FromUserName, Usernames = new List<string> { request.ToUserName } }, false);



            }
            catch (Exception ex)
            {
                chatResponse.Success = false;
                chatResponse.Message = ex.Message;
            }
        }

        public void LogTheChatRoom(LogTheChatRoomRequest request)
        {

            try
            {
                var chatCreatedTime = DateTimeOffset.FromUnixTimeMilliseconds(request.createDate).UtcDateTime.ToLocalTime();

                var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.fromChatRoom).FirstOrDefault();
                if (foundChatRoom == null)
                {
                    return;
                }
                var recievers = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName != request.fromUserName).Select(x => x.UserName).ToList();
                foreach (var user in recievers)
                {
                    ChatLog chatLog = new ChatLog
                    {
                        ChatGuid = Guid.NewGuid(),
                        ChatStatusId = request.status,
                        CreateDate = chatCreatedTime,
                        FromEmpId = 0,
                        ToEmPid = 0,
                        Message = request.message,
                        FromUserName = request.fromUserName,
                        ToUserName = user,
                        IsRtl = false

                    };
                    _chatLog.Add(chatLog);

                    ChatRoomLog chatRoomLog = new ChatRoomLog
                    {
                        ChatRoomId = foundChatRoom.Id,
                        ChatLogId = chatLog.Id
                    };
                    _chatRoomLog.Add(chatRoomLog);
                }
            }
            catch (Exception ex)
            {


            }


        }

        public List<ChatLog> GetChatHistory(GetChatHistoryRequest request)
        {

            var res = _chatLog.Find(x => x.FromEmpId == request.UserId || x.ToEmPid == request.UserId).OrderBy(x => x.CreateDate).ToList();
            return res;

        }

        public List<GetUnreadMessagesResponse> GetUnreadMessages(GetUnreadMessagesRequest request)
        {
            //GetUnreadMessagesResponse res = new GetUnreadMessagesResponse();
            var unreadLogs = _chatLog.Find(x => x.ToUserName == request.UserName && x.ChatStatusId == 1).GroupBy(x => x.FromUserName).Select(x => new GetUnreadMessagesResponse { UserName = x.Key, CountOfUnreadLogs = x.Count() }).ToList();
            //var resCount = _chatLog.Find(x => x.ToUserName == request.UserName && x.ChatStatusId == 1).Count();

            //res.UnreadLogs = unreadLogs;
            //res.CountOfUnreadLogs = resCount;
            return unreadLogs;


        }

        public UpdateUserToReadMessagResponse UpdateUserToReadMessag(UpdateUserToReadMessagRequest request)
        {
            UpdateUserToReadMessagResponse res = new UpdateUserToReadMessagResponse();

            try
            {
                var unreadLogs = _chatLog.Find(x => x.ToUserName == request.UserName && x.FromUserName == request.FromUserName && x.ChatStatusId == 1).ToList();
                unreadLogs.ForEach(x =>
                {
                    x.ChatStatusId = 2;
                });

                _context.BulkUpdate(unreadLogs);

                //for (var i = 0; i < unreadLogs.Count; i++)
                //{
                //    unreadLogs[i].ChatStatusId = 2;
                //    _chatLog.Update(unreadLogs[i]);
                //}
            }
            catch (Exception)
            {
                res.Success = false;
            }
            return res;

        }

        public GetLastMessageResponse GetLastMessage(GetLastMessageRequest request)
        {
            GetLastMessageResponse res = new GetLastMessageResponse();
            var lastLog = _chatLog.Find(x => x.FromUserName == request.fromUser && x.ToUserName == request.toUser).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            if (lastLog != null)
            {
                res.Message = lastLog.Message;
                res.CreatedDate = lastLog.CreateDate;
            }
            return res;

        }

        public async Task<HistoryMessageOfUsersResponse> HistoryMessageOfUsers(HistoryMessageOfUsersRequest request)
        {
            int pageNo = (--request.PageNumber);
            int skip = pageNo * request.PageSize;


            var res = new HistoryMessageOfUsersResponse();
            try
            {
                //var list1 = _chatLog.Find(x => x.FromUserName == request.fromUser && x.ToUserName == request.toUser).ToList();
                //var list2 = _chatLog.Find(x => x.FromUserName == request.toUser && x.ToUserName == request.fromUser).ToList();
                //var list4 = _chatLog.Find(x => (x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)).OrderByDescending(x => x.CreateDate).Skip(skip).Take(request.PageSize).ToList();
                var resDetailFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.fromUser });
                var fullNameFrom = resDetailFrom.result[0]["fullName"].ToString();

                var resDetailTo = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.toUser });
                var fullNameTo = resDetailTo.result[0]["fullName"].ToString();


                var list4 = _chatLog.Find(x => (x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)).Include(x => x.ChatStatus).OrderByDescending(x => x.CreateDate).Skip(skip).Take(request.PageSize).Select(x => new ResponseV2 { createDate = x.CreateDate, fromUserName = x.FromUserName, fullName = x.FromUserName == request.requestor ? fullNameTo : fullNameFrom, lastMessage = x.Message, pkEmployee = x.FromEmpId, isRtl = x.IsRtl, status = x.ChatStatus.ChatStatus1 }).ToList();
                var totalSize = _chatLog.Find(x => (x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)).Count();
                res.ChatLogs = list4;
                res.TotalSize = totalSize;

                //list1.AddRange(list2);
                //var list3 = list1.OrderBy(x => x.CreateDate).ToList();
                return res;
            }
            catch (Exception ex)
            {

                return res;
            }

        }

        public async Task<HistoryMessageOfUsersResponseV2> HistoryMessageOfUsersV2(HistoryMessageOfUsersRequestV2 request)
        {


            var res = new HistoryMessageOfUsersResponseV2();
            try
            {
                //var list1 = _chatLog.Find(x => x.FromUserName == request.fromUser && x.ToUserName == request.toUser).ToList();
                //var list2 = _chatLog.Find(x => x.FromUserName == request.toUser && x.ToUserName == request.fromUser).ToList();
                //var list4 = _chatLog.Find(x => (x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)).OrderByDescending(x => x.CreateDate).Skip(skip).Take(request.PageSize).ToList();
                var resDetailFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.fromUser });
                var fullNameFrom = resDetailFrom.result[0]["fullName"].ToString();

                var resDetailTo = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.toUser });
                var fullNameTo = resDetailTo.result[0]["fullName"].ToString();
                List<ResponseV2_V2> list4 = new List<ResponseV2_V2>();
                if (request.latestId != null)
                {

                    var foundByGuid = _chatLog.Find(x => x.ChatGuid == request.latestId).FirstOrDefault();

                    if (foundByGuid == null || foundByGuid.IsDeleted == true)
                    {
                        res.IsSuccess = false;
                        res.Message = "The guid not found";
                        return res;
                    }

                    if (request.isForward == true)
                    {
                        if (request.isIncluded == false)
                        {
                            list4 = _chatLog.Find(x => ((x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)) && x.Id > foundByGuid.Id && x.IsDeleted != true).Include(x => x.ChatStatus).OrderBy(x => x.CreateDate).Take(request.perPage).Select(x => new ResponseV2_V2 { messageId = x.ChatGuid, createDate = x.CreateDate.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = x.FromUserName == request.requestor ? fullNameTo : fullNameFrom, lastMessage = x.Message, pkEmployee = x.FromEmpId, isRtl = x.IsRtl, status = x.ChatStatus.ChatStatus1, toUserName = x.ToUserName, replyOf = x.Reply, forwardedBy = x.ForwardedBy }).ToList().OrderByDescending(x => x.createDate).ToList();
                        }
                        else
                        {
                            list4 = _chatLog.Find(x => ((x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)) && x.Id >= foundByGuid.Id && x.IsDeleted != true).Include(x => x.ChatStatus).OrderBy(x => x.CreateDate).Take(request.perPage).Select(x => new ResponseV2_V2 { messageId = x.ChatGuid, createDate = x.CreateDate.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = x.FromUserName == request.requestor ? fullNameTo : fullNameFrom, lastMessage = x.Message, pkEmployee = x.FromEmpId, isRtl = x.IsRtl, status = x.ChatStatus.ChatStatus1, toUserName = x.ToUserName, replyOf = x.Reply, forwardedBy = x.ForwardedBy }).ToList().OrderByDescending(x => x.createDate).ToList();

                        }
                    }
                    else
                    {
                        if (request.isIncluded == false)
                        {
                            list4 = _chatLog.Find(x => ((x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)) && x.Id < foundByGuid.Id && x.IsDeleted != true).Include(x => x.ChatStatus).OrderByDescending(x => x.CreateDate).Take(request.perPage).Select(x => new ResponseV2_V2 { messageId = x.ChatGuid, createDate = x.CreateDate.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = x.FromUserName == request.requestor ? fullNameTo : fullNameFrom, lastMessage = x.Message, pkEmployee = x.FromEmpId, isRtl = x.IsRtl, status = x.ChatStatus.ChatStatus1, toUserName = x.ToUserName, replyOf = x.Reply, forwardedBy = x.ForwardedBy }).ToList();
                        }
                        else
                        {
                            list4 = _chatLog.Find(x => ((x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)) && x.Id <= foundByGuid.Id && x.IsDeleted != true).Include(x => x.ChatStatus).OrderByDescending(x => x.CreateDate).Take(request.perPage).Select(x => new ResponseV2_V2 { messageId = x.ChatGuid, createDate = x.CreateDate.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = x.FromUserName == request.requestor ? fullNameTo : fullNameFrom, lastMessage = x.Message, pkEmployee = x.FromEmpId, isRtl = x.IsRtl, status = x.ChatStatus.ChatStatus1, toUserName = x.ToUserName, replyOf = x.Reply, forwardedBy = x.ForwardedBy }).ToList();

                        }

                    }
                    //var totalSize = _chatLog.Find(x => (x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)).Count();
                    var totalSize = 0;
                    res.ChatLogs = list4;
                    res.TotalSize = totalSize;
                    return SetAttachmnet(res);
                }
                else
                {
                    list4 = _chatLog.Find(x => ((x.FromUserName == request.fromUser && x.ToUserName == request.toUser) || (x.FromUserName == request.toUser && x.ToUserName == request.fromUser)) && x.Id > 0 && x.IsDeleted != true).Include(x => x.ChatStatus).OrderByDescending(x => x.CreateDate).Take(request.perPage).Select(x => new ResponseV2_V2 { messageId = x.ChatGuid, createDate = x.CreateDate.ToUnixTimeSeconds(), fromUserName = x.FromUserName, fullName = x.FromUserName == request.requestor ? fullNameTo : fullNameFrom, lastMessage = x.Message, pkEmployee = x.FromEmpId, isRtl = x.IsRtl, status = x.ChatStatus.ChatStatus1, toUserName = x.ToUserName, replyOf = x.Reply, forwardedBy = x.ForwardedBy }).ToList();
                    var totalSize = 0;
                    res.ChatLogs = list4;
                    res.TotalSize = totalSize;
                    return SetAttachmnet(res);
                }



                //list1.AddRange(list2);
                //var list3 = list1.OrderBy(x => x.CreateDate).ToList();

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
                return res;
            }

        }

        public HistoryMessageOfUsersResponseV2 SetAttachmnet(HistoryMessageOfUsersResponseV2 messageOfUsersResponseV2)
        {
            foreach (var i in messageOfUsersResponseV2.ChatLogs)
            {

                var messageGuid = i.messageId;
                var foundChatLog = _chatLog.Find(x => x.ChatGuid == messageGuid).FirstOrDefault();
                var foundChatLogId = foundChatLog.Id;
                var foundchatLogAttachment = _chatLogAttachment.Find(x => x.ChatLogId == foundChatLogId).FirstOrDefault();

                if (foundchatLogAttachment != null)
                {
                    var fileDetail = GetFileDetail(new GetFileDetailRequest { Id = foundchatLogAttachment.ChatAttachmentId });
                    i.attachment = fileDetail;
                }
            }

            return messageOfUsersResponseV2;
        }







        public CreateRoomResponse CreateChannelGroupRoom(CreateRoomRequest request)
        {
            CreateRoomResponse response = new CreateRoomResponse();
            var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName.Trim()).FirstOrDefault();
            if (foundChatRoom != null)
            {
                response.Message = "Room Name exists";
                response.IsSuccess = false;
                return response;
            }
            ChatRoomType? foundRoomType = null;
            if (request.IsChannel == true)
            {
                foundRoomType = _chatRoomType.Find(x => x.Name.ToLower() == "channel").FirstOrDefault();
            }
            else
            {
                foundRoomType = _chatRoomType.Find(x => x.Name.ToLower() == "group").FirstOrDefault();
            }

            ChatRoom chatRoom = new();
            chatRoom.ChatRoomName = request.ChatRoomName;
            chatRoom.CreateDatetime = DateTime.Now;
            chatRoom.CreatorUserName = request.CreatorUserName;
            chatRoom.ChatRoomTypeId = foundRoomType.Id;
            chatRoom.IsActive = true;
            chatRoom.Description = request.Description;
            chatRoom.ChatRoomLogoId = request.LogoId;
            _chatRoom.Add(chatRoom);

            ChatRoomMemeber chatRoomMemeber = new();
            chatRoomMemeber.CreateDateTime = DateTime.Now;
            chatRoomMemeber.UserName = request.CreatorUserName;
            chatRoomMemeber.UserId = 0;
            chatRoomMemeber.ChatRoomId = chatRoom.Id;
            chatRoomMemeber.IsActive = true;
            chatRoomMemeber.IsAdmin = true;
            _chatRoomMemeber.Add(chatRoomMemeber);

            var addmemberResult = AddMemberToChatRoom(chatRoom.Id, request.InviteeUserNames, request.CreatorUserName);
            response.Warnings.AddRange(addmemberResult);
            response.ChatRoomId = chatRoom.Id;
            return response;
        }

        public List<string> AddMemberToChatRoom(long chatRoomId, List<CreateRGUser> inviteeUserNames, string userName = "")
        {

            var foundRoom = _chatRoom.GetById(chatRoomId);
            List<string> result = new();
            //var userNames = inviteeUserNames.Split(",").Select(x => x.Trim()).ToList();
            var foundCreatorUser = _chatRoomMemeber.Find(x => x.ChatRoomId == chatRoomId && x.IsAdmin == true).FirstOrDefault();
            var isCreatorAdmin = false;

            if (foundCreatorUser != null)
            {
                isCreatorAdmin = true;
            }

            List<ChatRoomMemeber> chatRoomMembers = new List<ChatRoomMemeber>();
            foreach (var i in inviteeUserNames)
            {
                if (i.IsAdmin)
                {
                    if (!isCreatorAdmin)
                    {
                        result.Add($"{i.UserName} can not be set as admin because the requestor is not admin");
                        continue;
                    }
                }

                var foundChatRoomMember = _chatRoomMemeber.Find(x => x.ChatRoomId == chatRoomId && x.UserName == i.UserName).FirstOrDefault();

                if (foundChatRoomMember == null)
                {
                    ChatRoomMemeber temp = new();
                    temp.CreateDateTime = DateTime.Now;
                    temp.UserName = i.UserName;
                    temp.UserId = 0;
                    temp.ChatRoomId = chatRoomId;
                    temp.IsActive = true;
                    temp.IsAdmin = i.IsAdmin;
                    chatRoomMembers.Add(temp);
                }
                else
                {
                    result.Add($"{i} exists in {foundRoom.ChatRoomName} room");
                }
            }

            _chatRoomMemeber.AddRange(chatRoomMembers);
            return result;

        }

        public DisableRoomResponse DisableRoom(DisableRoomRequest request)
        {
            DisableRoomResponse response = new();
            try
            {
                var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.RoomName).FirstOrDefault();
                if (foundChatRoom == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Room not found";
                    return response;
                }
                if (foundChatRoom.CreatorUserName != request.OwnerName)
                {
                    response.IsSuccess = false;
                    response.Message = "User is not the owner of the room.";
                    return response;
                }

                foundChatRoom.IsActive = false;
                _chatRoom.Update(foundChatRoom);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }

            return response;

        }

        public CheckUserRolesResponse CheckUserRoles(CheckUserRolesRequest request)
        {
            CheckUserRolesResponse res = new();
            var theUser = _empMaster.Find(x => x.UserName == request.UserName && x.IsSuperUser == true).FirstOrDefault();
            if (theUser != null)
            {
                res.Roles.Add("SuperUser");
            }
            return res;
        }

        public GetUserRoomsResponse GetUserRooms(GetUserRoomsRequest request)
        {
            GetUserRoomsResponse response = new GetUserRoomsResponse();
            var roomList = _chatRoomMemeber.Find(x => x.UserName == request.UserName).Include(x => x.ChatRoom).Where(x => x.ChatRoom.IsActive == true).Select(x => x.ChatRoom.ChatRoomName).ToList();
            response.UserRooms = roomList;
            return response;
        }

        public List<GetUserRoomsResponse2> GetUserRoomsV2(GetUserRoomsRequest request)
        {
            var roomList = _chatRoomMemeber.Find(x => x.UserName == request.UserName).Include(x => x.ChatRoom).Where(x => x.ChatRoom.IsActive == true).Select(x => new GetUserRoomsResponse2 { RoomName = x.ChatRoom.ChatRoomName }).ToList();
            return roomList;
        }

        public List<GetUsersChatRoomsWithCountOfUnreadsResponse> GetUsersChatRoomsWithCountOfUnreads(string UserName)
        {

            var userRooms = GetUserRooms(new GetUserRoomsRequest { UserName = UserName }).UserRooms;

            //FnSpRequest request = new FnSpRequest();
            //request.FNSpName = "GSP_GetUsersRoomAndUreadMessages";
            //request.Parameters.Add(new ServiceOperatorParameter()
            //{
            //    Name = "UserName",
            //    Value = UserName
            //});
            //var res = _chatLog.GetAllFromSPWithOutput(request);
            //return res;



            GetAllFromSPWithOutputResponseViewModel res = new GetAllFromSPWithOutputResponseViewModel();
            List<GetUsersChatRoomsWithCountOfUnreadsResponse> res2 = new();
            var fnRes = _chatLog.PosGresFunction("GSP_GetUsersRoomAndUreadMessages", UserName);

            foreach (var i in fnRes)
            {
                var roomName = i["chatroomname"].ToString();
                var foundRoom = _chatRoom.Find(x => x.ChatRoomName == roomName).FirstOrDefault();
                var countOfRoomMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundRoom.Id).Count();
                var roomType = _chatRoomType.GetById(foundRoom.ChatRoomTypeId);
                var foundMember = _chatRoomMemeber.Find(x => x.UserName == UserName && x.ChatRoomId == foundRoom.Id).FirstOrDefault();
                res2.Add(new() { chatroomname = roomName, countOfMembers = countOfRoomMember, creator = foundRoom.CreatorUserName, isChannel = roomType.IsChannel, isAdmin = foundMember.IsAdmin });

            }



            //res.Result = fnRes;
            return res2;

        }







        public GetAllFromSPWithOutputResponseViewModel GetUsersChatroomMessages(GetUsersChatroomMessagesRequest request)
        {
            FnSpRequest fnSpRequest = new FnSpRequest();
            fnSpRequest.FNSpName = "GSP_GetUsersChatRoomMessages";
            fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
                new ServiceOperatorParameter(){Name="UserName" , Value=request.UserName },
                new ServiceOperatorParameter(){Name="RoomName ", Value=request.RoomName },
                new ServiceOperatorParameter(){Name="PageNumber" , Value= request.PageNumber},
                new ServiceOperatorParameter(){Name="PageSize" , Value= request.PageSize},
                new ServiceOperatorParameter(){Name="TotalSize" , IsOutPut=true, Value=""}
            });
            var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);
            return res;
        }

        public List<ChatLog> GetUsersChatroomMessagesV2(GetUsersChatroomMessagesRequest request)
        {
            //FnSpRequest fnSpRequest = new FnSpRequest();
            //fnSpRequest.FNSpName = "GSP_GetUsersChatRoomMessages";
            //fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
            //    new ServiceOperatorParameter(){Name="UserName" , Value=request.UserName },
            //    new ServiceOperatorParameter(){Name="RoomName ", Value=request.RoomName },
            //    new ServiceOperatorParameter(){Name="PageNumber" , Value= request.PageNumber},
            //    new ServiceOperatorParameter(){Name="PageSize" , Value= request.PageSize},
            //    new ServiceOperatorParameter(){Name="TotalSize" , IsOutPut=true, Value=""}
            //});
            //var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);
            int pageNo = (--request.PageNumber);
            int skip = pageNo * request.PageSize;
            var chatRoom = _chatRoom.Find(x => x.ChatRoomName == request.RoomName).FirstOrDefault();


            var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == chatRoom.Id).OrderByDescending(x => x.chatLog.CreateDate).Skip(skip).Take(request.PageSize).Select(x => x.chatLog).ToList();
            return tmpChatLog;
        }

        public async Task<AddToContactListResponse> AddToContactList(AddToContactListRequest request, bool checkUser = true)
        {
            var response = new AddToContactListResponse();
            try
            {

                //var ownerExits = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.OwnerUserName });
                //if (ownerExits.result.Count() == 0)
                //{
                //    response.Warnings.Add($"The Owner {request.OwnerUserName} does not exist in Central Database");
                //    return response;
                //}
                var ownerUserName = request.OwnerUserName.ToLower().Trim();
                var foundUser = _empMaster.Find(x => x.UserName == ownerUserName).FirstOrDefault();
                if (foundUser == null)
                {
                    foundUser = _empMaster.Add(new EmpMaster { UserName = ownerUserName });
                }

                request.Usernames = request.Usernames.Select(x => x.ToLower().Trim()).Distinct().ToList();
                request.Usernames.Remove(ownerUserName);

                List<UserContanct> userContancts = new List<UserContanct>();
                foreach (var item in request.Usernames)
                {
                    Response.GetEployeeDetails.GetEployeeDetailsResponse? memberExists = new Response.GetEployeeDetails.GetEployeeDetailsResponse();
                    if (checkUser == true)
                    {

                        memberExists = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = item });
                    }
                    if (memberExists.result.Count == 0 && checkUser == true)
                    {
                        response.Warnings.Add($"Member name {item} deos not exist in database");
                    }
                    else
                    {
                        var tmpContact = _userContanct.Find(x => x.UserName == item && x.EmpMasterId == foundUser.Id).FirstOrDefault();
                        if (tmpContact == null)
                        {
                            userContancts.Add(new UserContanct() { EmpMasterId = foundUser.Id, UserName = item });
                        }
                    }
                }
                _userContanct.AddRange(userContancts);

            }
            catch (Exception ex)
            {

                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public RemoveFromContactsResponse RemoveFromContacts(RemoveFromContactsRequest request)
        {
            RemoveFromContactsResponse response = new RemoveFromContactsResponse();
            try
            {
                var ownerUserName = request.OwnerUserName.ToLower().Trim();
                var foundUser = _empMaster.Find(x => x.UserName == request.OwnerUserName).FirstOrDefault();

                request.Usernames = request.Usernames.Select(x => x.ToLower().Trim()).Distinct().ToList();
                request.Usernames.Remove(ownerUserName);

                foreach (var item in request.Usernames)
                {
                    var foundContact = _userContanct.Find(x => x.EmpMasterId == foundUser.Id && x.UserName == item).FirstOrDefault();
                    if (foundContact != null)
                    {
                        _userContanct.Remove(foundContact);
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
        public async Task<GetContactListResponse> GetContactList(GetContactListRequest request)
        {
            int pageNo = (--request.PageNumber);
            int skip = pageNo * request.PageSize;

            var response = new GetContactListResponse();

            try
            {
                var ownerUserName = request.OwnerUserName.ToLower().Trim();

                var found = _empMaster.Find(x => x.UserName == ownerUserName).FirstOrDefault();
                if (found == null)
                {
                    found = _empMaster.Add(new EmpMaster { UserName = ownerUserName });
                }

                var foundContacts = _userContanct.Find(x => x.EmpMasterId == found.Id).Select(x => x.UserName).Skip(skip).Take(request.PageSize).ToList();
                var total = _userContanct.Find(x => x.EmpMasterId == found.Id).Count();
                var userNames = string.Join(",", foundContacts);

                var resFromGriffinApi = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = userNames });
                if (resFromGriffinApi.success == false)
                {
                    throw new Exception();
                }

                response.Contacts = resFromGriffinApi.result;
                AddLastSeenToGetContactListResult(response.Contacts);
                response.Total = total;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }
        private void AddLastSeenToGetContactListResult(List<Dictionary<string, object>> contanctListDict)
        {
            foreach (var i in contanctListDict)
            {
                var userName = i["userName"];
                var lastSeenRes = GetLastSeenOfEmp(new GetLastSeenOfEmpRequest { UserName = userName.ToString() });
                i.Add("lastSeen", lastSeenRes.lastSeenDate);

            }
        }

        public GetAllFromSPWithOutputResponseViewModel GetCountAndLastMessagePrivateMessage(GetCountAndLastMessagePrivateMessageRequest request)
        {
            FnSpRequest fnSpRequest = new FnSpRequest();
            fnSpRequest.FNSpName = "GSP_GetCountAndLastMessagePrivateMessage";
            fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
                new ServiceOperatorParameter(){Name="ToUserName" , Value=request.ToUserName },
                new ServiceOperatorParameter(){Name="PageNumber" , Value= request.PageNumber},
                new ServiceOperatorParameter(){Name="PageSize" , Value= request.PageSize},
                new ServiceOperatorParameter(){Name="TotalSize" , IsOutPut=true, Value=""}
            });

            var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);
            return res;
        }

        public GetAllFromSPWithOutputResponseViewModel GetCountAndLastMessagePrivateMessageV2(GetCountAndLastMessagePrivateMessageRequest request)
        {
            GetAllFromSPWithOutputResponseViewModel res = new GetAllFromSPWithOutputResponseViewModel();
            int pageNo = (--request.PageNumber);
            int skip = pageNo * request.PageSize;

            try
            {

                var tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName && x.ChatStatusId == 1).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).GroupBy(x => x.FromUserName).Select(x => new { count = x.Count(), fromUsername = x.Select(y => y.FromUserName).First() }).ToList();

                List<Dictionary<string, object>> theRes = new List<Dictionary<string, object>>();

                foreach (var i in tmpRes)
                {

                    var tmpChatLog = _chatLog.Find(x => (x.FromUserName == i.fromUsername && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i.fromUsername)).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).OrderByDescending(x => x.CreateDate).First();

                    Dictionary<string, object> tempDict = new Dictionary<string, object>();
                    tempDict.Add("fromUserName", i.fromUsername);
                    tempDict.Add("countOfUnreadMessage", i.count);
                    tempDict.Add("lastMessage", tmpChatLog.Message);
                    tempDict.Add("createDate", tmpChatLog.CreateDate);
                    tempDict.Add("sentUser", request.ToUserName);
                    theRes.Add(tempDict);
                }

                res.Result = theRes.OrderByDescending(x => Convert.ToInt32(x["countOfUnreadMessage"])).Skip(skip).Take(request.PageSize).ToList();
                res.OutPuts.Add("totalSize", theRes.Count);
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;
            }

            return res;
        }

        public async Task<GetAllEmployeeForChatResponse?> GetAllEmployeeForChat(GetUsersToChatRequest request)
        {
            var res = await _griffinAirAvation.GetAllEmployeeForChat(request);
            return res;
        }

        public List<string?> GetMastersOfContact(GetMastersOfContactRequest request)
        {
            var res = _userContanct.Find(x => x.UserName == request.UserName).Include(x => x.EmpMaster).Select(x => x.EmpMaster.UserName).ToList();
            return res;
        }

        public ResponseMessage SetLastEmpLastSeen(SetLastEmpLastSeenRequest request)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                var lastSeendt = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(request.LastSeenDate)).UtcDateTime.ToLocalTime();
                EmpMaster? empMaster = null;

                empMaster = _empMaster.Find(x => x.UserName == request.UserName).FirstOrDefault();
                if (empMaster == null)
                {
                    var tmpEmpMaster = new EmpMaster { UserName = request.UserName };
                    empMaster = _empMaster.Add(tmpEmpMaster);
                }
                empMaster.LastSeenDate = lastSeendt;
                _empMaster.Update(empMaster);
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
            }
            return response;
        }

        public GetLastSeenOfEmpResponse GetLastSeenOfEmp(GetLastSeenOfEmpRequest request)
        {
            GetLastSeenOfEmpResponse response = new GetLastSeenOfEmpResponse();
            try
            {
                EmpMaster? empMaster = null;

                empMaster = _empMaster.Find(x => x.UserName == request.UserName).FirstOrDefault();
                if (empMaster == null)
                {
                    var tmpEmpMaster = new EmpMaster { UserName = request.UserName };
                    empMaster = _empMaster.Add(tmpEmpMaster);
                }
                if (empMaster.LastSeenDate != null)
                {
                    var res = (long)(empMaster.LastSeenDate.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    response.lastSeenDate = res.ToString();
                }
                else
                {
                    response.lastSeenDate = null;
                }

            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
            }
            return response;

        }

        public async Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryV2(GetCountAndLastMessagePrivateMessageRequest request)
        {
            GetAllFromSPWithOutputResponseViewModel res = new GetAllFromSPWithOutputResponseViewModel();
            int pageNo = (--request.PageNumber);
            int skip = pageNo * request.PageSize;

            try
            {
                var tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).OrderByDescending(x => x.CreateDate).GroupBy(x => x.FromUserName).Select(x => new { fromUserName = x.Select(y => y.FromUserName).First(), createdate = x.Max(y => y.CreateDate) }).Skip(skip).Take(request.PageSize).ToList();

                List<Dictionary<string, object>> theRes = new List<Dictionary<string, object>>();
                foreach (var i in tmpRes)
                {
                    var lastMessage = _chatLog.Find(x => x.FromUserName == i.fromUserName && x.CreateDate == i.createdate).First().Message;
                    var count = _chatLog.Find(x => x.FromUserName == i.fromUserName && x.ToUserName == request.ToUserName).Count();

                    Dictionary<string, object> tempDict = new Dictionary<string, object>();
                    tempDict.Add("fromUserName", i.fromUserName);
                    tempDict.Add("countOfUnreadMessage", count);
                    tempDict.Add("lastMessage", lastMessage);
                    tempDict.Add("createDate", i.createdate);

                    theRes.Add(tempDict);
                }
                var totalSize = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).OrderByDescending(x => x.CreateDate).GroupBy(x => x.FromUserName).Select(x => new { fromUserName = x.Select(y => y.FromUserName).First(), createdate = x.Max(y => y.CreateDate) }).Count();
                res.Result = theRes;
                res.OutPuts.Add("totalSize", totalSize);
                await AddPkEmployeeToResult(res.Result);

                //FnSpRequest fnSpRequest = new FnSpRequest();
                //fnSpRequest.FNSpName = "GSP_GetUserChatHistoryV2";
                //fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
                //    new ServiceOperatorParameter(){Name="ToUserName" , Value=request.ToUserName },
                //    new ServiceOperatorParameter(){Name="PageNumber" , Value= request.PageNumber},
                //    new ServiceOperatorParameter(){Name="PageSize" , Value= request.PageSize},
                //    new ServiceOperatorParameter(){Name="TotalSize" , IsOutPut=true, Value=""}
                //});

                //var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }

            return res;
        }

        public async Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryV3(GetCountAndLastMessagePrivateMessageRequest request)
        {

            var res = new GetAllFromSPWithOutputResponseViewModel();
            int pageNo = (--request.PageNumber);
            int skip = pageNo * request.PageSize;
            try
            {
                //    FnSpRequest fnSpRequest = new FnSpRequest();
                //    fnSpRequest.FNSpName = "GSP_GetUserChatHistoryV3";
                //    fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
                //    new ServiceOperatorParameter(){Name="ToUserName" , Value=request.ToUserName },
                //    new ServiceOperatorParameter(){Name="PageNumber" , Value= request.PageNumber},
                //    new ServiceOperatorParameter(){Name="PageSize" , Value= request.PageSize},
                //    new ServiceOperatorParameter(){Name="TotalSize" , IsOutPut=true, Value=""}
                //});

                //var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);


                //var tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).GroupBy(x => x.FromUserName).Select(x => new { createDate = x.Max(y => y.CreateDate), userName = x.Select(y => y.FromUserName).First(), id = x.Select(y => y.Id).Max(), message = getlast( x.Select(y => y.Message).ToList()) }).ToList();
                //var tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).GroupBy(x => x.FromUserName).Select(x => new { createDate = x.Max(y => y.CreateDate), userName = x.Select(y => y.FromUserName).First(), id = x.Select(y => y.Id).Max()}).ToList();
                var tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).GroupBy(x => x.FromUserName).Select(x => x.Select(y => y.FromUserName).First()).ToList();
                var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.Id, y => y.ChatStatus1);
                var theRes = new List<Dictionary<string, object>>();
                //var temres2 = _chatLog.Find(x => tmpRes.Contains(x.FromUserName) || x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).GroupBy() .ToList();
                foreach (var i in tmpRes)
                {
                    Dictionary<string, object> tempDict = new Dictionary<string, object>();

                    //var tmpChatLog1 = _context.ChatLogs.Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Include(x => x.ChatStatus).Where(x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                    //var tmpChatLog2 = _context.ChatLogs.Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Include(x => x.ChatStatus).Where(x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).OrderByDescending(x => x.CreateDate).FirstOrDefault();

                    //var tmpChatLog3 = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog=>chatLog.Id , chatRoomLog=> chatRoomLog.ChatLogId, (chatLog, chatRoomLog)=> new { chatLog, chatRoomLog }).Where(x=>x.chatRoomLog.ChatRoomId==12 && (x.chatLog.FromUserName == i && x.chatLog.ToUserName == request.ToUserName) || (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i)).OrderByDescending(x=> x.chatLog.CreateDate).FirstOrDefault();

                    var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == 12).Where(x => (x.chatLog.FromUserName == i && x.chatLog.ToUserName == request.ToUserName) || (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i)).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();


                    //var tmpChatLog4 = _chatLog.Find(x => (x.FromUserName == i && x.ToUserName == request.ToUserNa//me) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).Include(x => x.ChatRoomLogs).Include(x => x.ChatStatus).OrderByDescending(x => x.CreateDate).ToList();
                    //var tmpChatLog5 = _context.ChatLogs.Where(x=> x.ChatRoomLogs.Where(y=> y.ChatRoomId==12));

                    //var tmpChatLog5 = _chatLog.Find(x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Include(x => x.ChatStatus).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                    var countOfUnreadMessage = _chatLog.Find(x => x.FromUserName == i && x.ToUserName == request.ToUserName && x.ChatStatusId == 1).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Count();
                    tempDict.Add("fromUserName", i);
                    tempDict.Add("countOfUnreadMessage", countOfUnreadMessage);
                    tempDict.Add("lastMessage", tmpChatLog.Message);
                    tempDict.Add("createDate", tmpChatLog.CreateDate);
                    tempDict.Add("sentUser", tmpChatLog.FromUserName);
                    //tempDict.Add("status", tmpChatLog.ChatStatusId == 1 ? "unSeen" : "seen");
                    tempDict.Add("status", chatStatusDict[tmpChatLog.ChatStatusId]);
                    theRes.Add(tempDict);
                }

                var fromUserNames = theRes.Select(x => x["fromUserName"].ToString()).ToList();

                //var theRes2 = _chatLog.Find(x => x.FromUserName == request.ToUserName && !fromUserNames.Contains(x.FromUserName)).ToList();
                var theRes2 = _chatLog.Find(x => x.FromUserName == request.ToUserName && !fromUserNames.Contains(x.ToUserName)).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Select(x => x.ToUserName).Distinct().ToList();
                var theRes3 = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == 12).Where(x => x.chatLog.FromUserName == request.ToUserName && !fromUserNames.Contains(x.chatLog.ToUserName)).Select(x => x.chatLog.ToUserName).Distinct().ToList();

                foreach (var i in theRes2)
                {
                    Dictionary<string, object> tempDict = new Dictionary<string, object>();
                    var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == 12).Where(x => (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i)).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();
                    //var countOfUnreadMessage1 = _chatLog.Find(x => (x.FromUserName == request.ToUserName && x.ToUserName == i && x.ChatStatusId == 1)).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).Count();
                    if (tmpChatLog != null)
                    {
                        var countOfUnreadMessage = _chatLog.Find(x => (x.FromUserName == i && x.ToUserName == request.ToUserName && x.ChatStatusId == 1)).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Count();
                        tempDict.Add("fromUserName", i);
                        tempDict.Add("countOfUnreadMessage", countOfUnreadMessage);
                        tempDict.Add("lastMessage", tmpChatLog.Message);
                        tempDict.Add("createDate", tmpChatLog.CreateDate);
                        tempDict.Add("sentUser", request.ToUserName);
                        tempDict.Add("status", chatStatusDict[tmpChatLog.ChatStatusId]);

                        theRes.Add(tempDict);
                    }
                }

                var totalSize = theRes.Count();
                res.OutPuts.Add("totalSize", totalSize);
                //res.Result = theRes.OrderByDescending(x => Convert.ToDateTime(x["createDate"])).Skip(skip).Take(request.PageSize).ToList();
                res.Result = theRes.OrderByDescending(x => DateTimeOffset.Parse(x["createDate"].ToString())).Skip(skip).Take(request.PageSize).ToList();
                await AddFullNameToResult(res.Result);
                //DateTimeOffset.Parse()


                //var tmpResStr =  JsonConvert.SerializeObject(tmpRes);
                //var lstdictRes =  JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(tmpResStr);
                //var tmpRes1 = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).OrderByDescending(x => x.Id).ToList();//.GroupBy(x=> x.FromUserName).Select(x=> new { ddd= x.Max(y=>y.CreateDate), mmm=x.Select(y=>y.FromUserName) }).ToList();
                //res.Result = lstdictRes;
                //res.ResObj= tmpRes;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            // _chatLog.Find(x => x.ToUserName == request.ToUserName).GroupBy(x=> x.FromUserName).Select(x=> )
            //await AddPkEmployeeToResult(res.Result);
            return res;
        }



        public async Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryV3_V2(GetCountAndLastMessagePrivateMessageRequestV2 request)
        {
            var fromDatetime = DateTimeOffset.FromUnixTimeMilliseconds(request.FromTimeStamp);
            var res = new GetAllFromSPWithOutputResponseViewModel();

            //int pageNo = (--request.PageNumber);
            //int skip = pageNo * request.PageSize;

            try
            {
                //    FnSpRequest fnSpRequest = new FnSpRequest();
                //    fnSpRequest.FNSpName = "GSP_GetUserChatHistoryV3";
                //    fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
                //    new ServiceOperatorParameter(){Name="ToUserName" , Value=request.ToUserName },
                //    new ServiceOperatorParameter(){Name="PageNumber" , Value= request.PageNumber},
                //    new ServiceOperatorParameter(){Name="PageSize" , Value= request.PageSize},
                //    new ServiceOperatorParameter(){Name="TotalSize" , IsOutPut=true, Value=""}
                //});

                //var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);


                //var tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).GroupBy(x => x.FromUserName).Select(x => new { createDate = x.Max(y => y.CreateDate), userName = x.Select(y => y.FromUserName).First(), id = x.Select(y => y.Id).Max(), message = getlast( x.Select(y => y.Message).ToList()) }).ToList();
                //var tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).GroupBy(x => x.FromUserName).Select(x => new { createDate = x.Max(y => y.CreateDate), userName = x.Select(y => y.FromUserName).First(), id = x.Select(y => y.Id).Max()}).ToList();
                List<string?> tmpRes = new List<string?>();
                if (request.FromTimeStamp == 0)
                {
                    tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName && x.IsDeleted != true).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).GroupBy(x => x.FromUserName).Select(x => x.Select(y => y.FromUserName).First()).ToList();
                }
                else
                {
                    tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName && x.ClientDateTime > fromDatetime && x.IsDeleted != true).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).GroupBy(x => x.FromUserName).Select(x => x.Select(y => y.FromUserName).First()).ToList();
                }
                var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.Id, y => y.ChatStatus1);
                var theRes = new List<Dictionary<string, object>>();
                //var temres2 = _chatLog.Find(x => tmpRes.Contains(x.FromUserName) || x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).GroupBy() .ToList();
                var tmpChatLogList = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == 12).Where(x => (tmpRes.Contains(x.chatLog.FromUserName) && x.chatLog.ToUserName == request.ToUserName && x.chatLog.IsDeleted != true) || (x.chatLog.FromUserName == request.ToUserName && tmpRes.Contains(x.chatLog.ToUserName) && x.chatLog.IsDeleted != true)).OrderByDescending(x => x.chatLog.ClientDateTime).Select(x => x.chatLog).ToList();
                foreach (var i in tmpRes)
                {
                    Dictionary<string, object> tempDict = new Dictionary<string, object>();

                    //var tmpChatLog1 = _context.ChatLogs.Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Include(x => x.ChatStatus).Where(x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                    //var tmpChatLog2 = _context.ChatLogs.Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Include(x => x.ChatStatus).Where(x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).OrderByDescending(x => x.CreateDate).FirstOrDefault();

                    //var tmpChatLog3 = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog=>chatLog.Id , chatRoomLog=> chatRoomLog.ChatLogId, (chatLog, chatRoomLog)=> new { chatLog, chatRoomLog }).Where(x=>x.chatRoomLog.ChatRoomId==12 && (x.chatLog.FromUserName == i && x.chatLog.ToUserName == request.ToUserName) || (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i)).OrderByDescending(x=> x.chatLog.CreateDate).FirstOrDefault();

                    //var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == 12).Where(x => (x.chatLog.FromUserName == i && x.chatLog.ToUserName == request.ToUserName && x.chatLog.IsDeleted != true) || (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i && x.chatLog.IsDeleted != true)).OrderByDescending(x => x.chatLog.ClientDateTime).Select(x => x.chatLog).FirstOrDefault();

                    var tmpChatLog = tmpChatLogList.Where(x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).OrderByDescending(x => x.ClientDateTime).FirstOrDefault();

                    //var tmpChatLog4 = _chatLog.Find(x => (x.FromUserName == i && x.ToUserName == request.ToUserNa//me) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).Include(x => x.ChatRoomLogs).Include(x => x.ChatStatus).OrderByDescending(x => x.CreateDate).ToList();
                    //var tmpChatLog5 = _context.ChatLogs.Where(x=> x.ChatRoomLogs.Where(y=> y.ChatRoomId==12));

                    //var tmpChatLog5 = _chatLog.Find(x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Include(x => x.ChatStatus).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                    int countOfUnreadMessage = 0;
                    if (request.FromTimeStamp == 0)
                    {
                        countOfUnreadMessage = _chatLog.Find(x => x.FromUserName == i && x.ToUserName == request.ToUserName && x.ChatStatusId == 1 && x.IsDeleted != true).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Count();
                    }
                    else
                    {
                        countOfUnreadMessage = _chatLog.Find(x => x.FromUserName == i && x.ToUserName == request.ToUserName && x.ChatStatusId == 1 && x.ClientDateTime > fromDatetime && x.IsDeleted != true).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Count();
                    }

                    tempDict.Add("roomName", "private");
                    tempDict.Add("roomType", "private");
                    tempDict.Add("lastMessageId", tmpChatLog.ChatGuid);
                    tempDict.Add("userName", i);
                    tempDict.Add("countOfUnreadMessage", countOfUnreadMessage);
                    tempDict.Add("lastMessage", tmpChatLog.Message);
                    tempDict.Add("createDate", tmpChatLog.ClientDateTime.Value.ToUnixTimeMilliseconds());
                    tempDict.Add("fromUserName", tmpChatLog.FromUserName);
                    tempDict.Add("toUserName", tmpChatLog.ToUserName);
                    //tempDict.Add("status", tmpChatLog.ChatStatusId == 1 ? "unSeen" : "seen");
                    tempDict.Add("status", chatStatusDict[tmpChatLog.ChatStatusId]);
                    tempDict.Add("forwardedBy", tmpChatLog.ForwardedBy);
                    tempDict.Add("replyOf", tmpChatLog.Reply);

                    theRes.Add(tempDict);
                }

                var fromUserNames = theRes.Select(x => x["userName"].ToString()).ToList();

                //var theRes2 = _chatLog.Find(x => x.FromUserName == request.ToUserName && !fromUserNames.Contains(x.FromUserName)).ToList();
                List<string?> theRes2 = new List<string?>();
                if (request.FromTimeStamp == 0)
                {
                    theRes2 = _chatLog.Find(x => x.FromUserName == request.ToUserName && !fromUserNames.Contains(x.ToUserName) && x.IsDeleted != true).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Select(x => x.ToUserName).Distinct().ToList();
                }
                else
                {
                    theRes2 = _chatLog.Find(x => x.FromUserName == request.ToUserName && !fromUserNames.Contains(x.ToUserName) && x.ClientDateTime > fromDatetime && x.IsDeleted != true).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Select(x => x.ToUserName).Distinct().ToList();
                }
                //var theRes3 = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == 12).Where(x => x.chatLog.FromUserName == request.ToUserName && !fromUserNames.Contains(x.chatLog.ToUserName) && x.chatLog.IsDeleted != true).Select(x => x.chatLog.ToUserName).Distinct().ToList();

                var tmpChatLogList2 = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == 12).Where(x => (x.chatLog.FromUserName == request.ToUserName && theRes2.Contains(x.chatLog.ToUserName) && x.chatLog.IsDeleted != true)).OrderByDescending(x => x.chatLog.ClientDateTime).Select(x => x.chatLog).ToList();

                foreach (var i in theRes2)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        Dictionary<string, object> tempDict = new Dictionary<string, object>();
                        //var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == 12).Where(x => (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i && x.chatLog.IsDeleted != true)).OrderByDescending(x => x.chatLog.ClientDateTime).Select(x => x.chatLog).FirstOrDefault();

                        var tmpChatLog = tmpChatLogList2.Where(x => (x.FromUserName == request.ToUserName && x.ToUserName == i)).OrderByDescending(x => x.ClientDateTime).FirstOrDefault();
                        //var countOfUnreadMessage1 = _chatLog.Find(x => (x.FromUserName == request.ToUserName && x.ToUserName == i && x.ChatStatusId == 1)).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).Count();
                        int countOfUnreadMessage = 0;
                        if (request.FromTimeStamp == 0)
                        {
                            countOfUnreadMessage = _chatLog.Find(x => (x.FromUserName == i && x.ToUserName == request.ToUserName && x.ChatStatusId == 1 && x.IsDeleted != true)).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Count();
                        }
                        else
                        {
                            countOfUnreadMessage = _chatLog.Find(x => (x.FromUserName == i && x.ToUserName == request.ToUserName && x.ChatStatusId == 1 && x.ClientDateTime >= fromDatetime && x.IsDeleted != true)).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).Count();
                        }
                        tempDict.Add("roomName", "private");
                        tempDict.Add("roomType", "private");
                        tempDict.Add("lastMessageId", tmpChatLog.ChatGuid);
                        tempDict.Add("userName", i);
                        tempDict.Add("countOfUnreadMessage", countOfUnreadMessage);
                        tempDict.Add("lastMessage", tmpChatLog.Message);
                        tempDict.Add("createDate", tmpChatLog.ClientDateTime.Value.ToUnixTimeMilliseconds());
                        tempDict.Add("fromUserName", request.ToUserName);
                        tempDict.Add("toUserName", tmpChatLog.ToUserName);
                        tempDict.Add("status", chatStatusDict[tmpChatLog.ChatStatusId]);
                        tempDict.Add("forwardedBy", tmpChatLog.ForwardedBy);
                        tempDict.Add("replyOf", tmpChatLog.Reply);

                        theRes.Add(tempDict);
                    }
                }

                var totalSize = theRes.Count();
                res.OutPuts.Add("totalSize", totalSize);
                //res.Result = theRes.OrderByDescending(x => Convert.ToDateTime(x["createDate"])).Skip(skip).Take(request.PageSize).ToList();

                //res.Result = theRes.OrderByDescending(x => DateTimeOffset.Parse(x["createDate"].ToString())).Skip(skip).Take(request.PageSize).ToList();
                //res.Result = theRes.OrderByDescending(x => DateTimeOffset.Parse(x["createDate"].ToString())).ToList();
                res.Result = theRes.OrderByDescending(x => Convert.ToInt64(x["createDate"])).ToList();

                await AddFullNameToResult(res.Result);
                //DateTimeOffset.Parse()
                AddFileAttachmentToGetUserChatHistory(res.Result);
                CheckIfAdmin(res.Result);

                //var tmpResStr =  JsonConvert.SerializeObject(tmpRes);
                //var lstdictRes =  JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(tmpResStr);
                //var tmpRes1 = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).OrderByDescending(x => x.Id).ToList();//.GroupBy(x=> x.FromUserName).Select(x=> new { ddd= x.Max(y=>y.CreateDate), mmm=x.Select(y=>y.FromUserName) }).ToList();
                //res.Result = lstdictRes;
                //res.ResObj= tmpRes;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            // _chatLog.Find(x => x.ToUserName == request.ToUserName).GroupBy(x=> x.FromUserName).Select(x=> )
            //await AddPkEmployeeToResult(res.Result);
            var resChatRoom = await GetUsersChatRoomsWithCountOfUnreadsV4(new ListOfRoomsWithUnreadMessagesRequestV3 { fromTimeStamp = request.FromTimeStamp, userName = request.ToUserName });
            res.Result.AddRange(resChatRoom.UserChatRoomsWithCountOfUnreads);
            return res;
        }

        public async Task<PoolingResponse> Pooling(GetCountAndLastMessagePrivateMessageRequestV2 request)
        {

            PoolingResponse res = new();
            try
            {
                res.list = await GetUserChatHistoryV3_V2(request);


                HistoryChatOfUsersPoolingRequest req = new HistoryChatOfUsersPoolingRequest();
                req.UserName = request.ToUserName;
                List<HistoryChatRequestDetailPooling> requestDetailPoolingList = new();

                foreach (var i in res.list.Result)
                {
                    HistoryChatRequestDetailPooling tmp = new()
                    {
                        TimeStamp = request.FromTimeStamp,
                        type = i["roomType"].ToString(),
                        name = i["userName"].ToString(),
                        roomName = i["roomName"].ToString()
                    };
                    requestDetailPoolingList.Add(tmp);

                }
                req.RquestDetails = requestDetailPoolingList;

                res.messages = await HistoryChatOfUsersPooling(req);
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;

        }

        //public async Task<ChatReplyMessage> SetChatReplyMessageAsync(Guid? guid)
        //{
        //    if (guid == null)
        //    {
        //        return null;
        //    }

        //    ChatReplyMessage res = new();
        //    var foundChat = _chatLog.Find(x => x.ChatGuid == guid).FirstOrDefault();
        //    if (foundChat == null)
        //    {
        //        return null;
        //    }

        //    var chatLogAttachment = _chatLogAttachment.Find(x => x.ChatLogId == foundChat.Id).FirstOrDefault();
        //    if (chatLogAttachment != null)
        //    {
        //        var foundAttachment = _chatAttachment.Find(x => x.Id == chatLogAttachment.ChatAttachmentId).FirstOrDefault();
        //        var fileDetail = GetFileDetail(new GetFileDetailRequest { Id = foundAttachment.Id });
        //        //i["attachment"] = fileDetail;
        //        res.FileType = fileDetail.type;
        //    }

        //    res.Username = foundChat.;
        //    res.Msg = foundChat.Message;
        //    //res.
        //    var empDetail = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = res.Username });
        //    res.FullName = empDetail.result[0]["fullName"].ToString();
        //    res.Guid = guid;

        //    return res;

        //}

        public void CheckIfAdmin(List<Dictionary<string, object>> chats)
        {
            var roomList = new List<string>();
            foreach (var i in chats)
            {
                if (i["roomType"] != "private")
                {
                    roomList.Add(i["roomName"].ToString());
                }
            }
            var chatRooms = _chatRoom.Find(x => roomList.Contains(x.ChatRoomName)).ToList();
            var chatRoomIds = chatRooms.Select(x => x.Id);
            var chatRoomMembers = _chatRoomMemeber.Find(x => chatRoomIds.Contains(x.ChatRoomId)).ToList();

            foreach (var i in chats)
            {
                if (i["roomType"] == "private")
                {
                    i.Add("isAdmin", true);
                }
                else
                {
                    var roomName = i["roomName"].ToString();
                    var userName = i["userName"].ToString();
                    var foundRoom = chatRooms.Where(x => x.ChatRoomName == roomName).FirstOrDefault();

                    var chekIfIsAdmin = chatRoomMembers.Where(x => x.ChatRoomId == foundRoom.Id && x.UserName == userName && x.IsAdmin == true).FirstOrDefault();
                    if (chekIfIsAdmin == null)
                    {
                        i.Add("isAdmin", false);
                    }
                    else
                    {
                        i.Add("isAdmin", true);
                    }
                }
            }
        }

        public void AddFileAttachmentToGetUserChatHistory(List<Dictionary<string, object>> chats)
        {

            var chatGuids = chats.Select(x => Guid.Parse(x["lastMessageId"].ToString())).ToList();
            var chatLogs = _chatLog.Find(x => chatGuids.Contains(x.ChatGuid)).ToList();
            var chatLogIds = chatLogs.Select(x => x.Id).ToList();

            var chatLogAttachments = _chatLogAttachment.Find(x => chatLogIds.Contains(x.ChatLogId)).ToList();
            var chatAttachmentIds = chatLogAttachments.Select(x => x.ChatAttachmentId).ToList();

            var foundAttachments = _chatAttachment.Find(x => chatAttachmentIds.Contains(x.Id));


            foreach (var i in chats)
            {
                var chatGuid = Guid.Parse(i["lastMessageId"].ToString());
                var foundChatLog = chatLogs.Where(x => x.ChatGuid == chatGuid).FirstOrDefault();
                var chatLogAttachment = chatLogAttachments.Where(x => x.ChatLogId == foundChatLog.Id).FirstOrDefault();
                if (chatLogAttachment != null)
                {
                    var foundAttachment = foundAttachments.Where(x => x.Id == chatLogAttachment.ChatAttachmentId).FirstOrDefault();
                    GetFileDetailResponse getFileDetailResponse = new GetFileDetailResponse
                    {
                        contentType = foundAttachment.ContentType,
                        id = foundAttachment.Id,
                        name = foundAttachment.FileName,
                        size = foundAttachment.FileSize ?? 0,
                        type = foundAttachment.FileType

                    };
                    var fileDetail = getFileDetailResponse;

                    i["attachment"] = fileDetail;
                }

            }

        }
        static string getlast(IEnumerable<string> strings)
        {
            return strings.Last();
        }

        private async Task AddPkEmployeeToResult(List<Dictionary<string, object>> theDict)
        {
            foreach (var i in theDict)
            {
                var fromUserName = i["fromUserName"].ToString();
                var empDetail = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = fromUserName });
                var pkemp = empDetail.result[0]["pkEmployee"];
                i.Add("pkEmployee", pkemp);
                i.Add("fullName", empDetail.result[0]["fullName"]);

            }

        }
        private async Task AddFullNameToResult(List<Dictionary<string, object>> theDict)
        {
            List<string> fromUserNames = new();
            foreach (var i in theDict)
            {
                fromUserNames.Add(i["userName"].ToString().Trim());
            }
            fromUserNames = fromUserNames.Distinct().ToList();
            string commaSeperatedUserNames = string.Join(",", fromUserNames);


            var resDetailFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = commaSeperatedUserNames });
            Dictionary<string, string[]> resdict = new();
            foreach (var i in resDetailFrom.result)
            {
                resdict.Add(i["userName"].ToString(), [i["pkEmployee"].ToString(), i["fullName"].ToString()]);
            }



            var counterIndex = -1;
            List<int> notFoundUserIndexes = new();
            foreach (var i in theDict)
            {
                counterIndex++;
                var fromUserName = i["userName"].ToString();
                //Response.GetEployeeDetails.GetEployeeDetailsResponse? empDetail = new();

                //empDetail = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = fromUserName });

                if (!resdict.ContainsKey(i["userName"].ToString()))
                {
                    notFoundUserIndexes.Add(counterIndex);

                    continue;
                }
                else
                {
                    i.Add("fullName", resdict[i["userName"].ToString()][1]);

                    i.Add("pkEmployee", resdict[i["userName"].ToString()][0]);
                }

            }

            foreach (var i in notFoundUserIndexes)
            {
                theDict.RemoveAt(i);
            }
        }

        public async Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryUnreadMessage(GetUserChatHistoyUreadMessageRequest request)
        {
            FnSpRequest fnSpRequest = new FnSpRequest();
            fnSpRequest.FNSpName = "GSP_GetUserChatHistoryUnreadMessage";
            fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
                new ServiceOperatorParameter(){Name="ToUserName" , Value=request.ToUserName }
            });
            var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);
            await AddPkEmployeeToResult(res.Result);
            return res;
        }

        public async Task<GetAllFromSPWithOutputResponseViewModel> GetUserChatHistoryUnreadMessageV2(GetUserChatHistoyUreadMessageRequest request)
        {
            var res = new GetAllFromSPWithOutputResponseViewModel();

            //FnSpRequest fnSpRequest = new FnSpRequest();
            //fnSpRequest.FNSpName = "GSP_GetUserChatHistoryUnreadMessageV2";
            //fnSpRequest.Parameters.AddRange(new List<ServiceOperatorParameter>() {
            //    new ServiceOperatorParameter(){Name="ToUserName" , Value=request.ToUserName }
            //});
            try
            {
                var tmpRes = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatRoomLogs.Where(y => y.ChatLogId == 12)).GroupBy(x => x.FromUserName).Select(x => x.Select(y => y.FromUserName).First()).ToList();
                var theRes = new List<Dictionary<string, object>>();
                foreach (var i in tmpRes)
                {
                    Dictionary<string, object> tempDict = new Dictionary<string, object>();
                    var tmpChatLog = _chatLog.Find(x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i)).OrderByDescending(x => x.CreateDate).FirstOrDefault();
                    var countOfUnreadMessage = _chatLog.Find(x => x.FromUserName == i && x.ToUserName == request.ToUserName && x.ChatStatusId == 1).Count();

                    if (countOfUnreadMessage > 0)
                    {

                        tempDict.Add("fromUserName", i);
                        tempDict.Add("countOfUnreadMessage", countOfUnreadMessage);
                        tempDict.Add("lastMessage", tmpChatLog.Message);
                        tempDict.Add("createDate", tmpChatLog.CreateDate);
                        tempDict.Add("sentUser", tmpChatLog.FromUserName);
                        theRes.Add(tempDict);
                    }

                }


                //var res = _chatLog.GetAllFromSPWithOutput(fnSpRequest);
                res.Result = theRes;

                await AddPkEmployeeToResult(res.Result);
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;
            }

            return res;
        }

        public CountOfUnreadMessageResponse CountOfUnreadMessage(string fromUserName, string toUserName)
        {
            CountOfUnreadMessageResponse response = new CountOfUnreadMessageResponse();

            try
            {
                var res = _chatLog.Find(x => x.FromUserName == fromUserName && x.ToUserName == toUserName && x.ChatStatusId == 1).Count();
                response.CountOfUnreadMessage = res;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public CountOfChatRoomUnreadMessageResponse CountOfChatRoomUnreadMessage(CountOfChatRoomUnreadMessageRequest request)
        {
            CountOfChatRoomUnreadMessageResponse res = new();

            var chatStatus = _chatStatus.Find(x => true).ToDictionary(x => x.ChatStatus1, x => x.Id);
            var foundRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();
            if (foundRoom == null)
            {
                res.IsSuccess = false;
                res.Message = "Room Not Found";
                return res;
            }


            var CountOfUnreadMessage = _userChatRoomReciever.Find(x => x.ChatroomId == foundRoom.Id && x.UserName == request.UserName && x.ChatroomId == chatStatus["sent"]).Count();
            return res;
        }

        public async void FileUploadPrivateChat(FileUploadPrivateChatRequest request)
        {
            var fromUserDetail = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.FromUserName });
            var toUserDetail = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = request.ToUserName });

            var pkFromEmp = fromUserDetail.result[0]["pkEmployee"];
            var pkToEmp = toUserDetail.result[0]["pkEmployee"];

            ChatLog chatLog = new();
            chatLog.FromUserName = request.FromUserName;
            chatLog.ToUserName = request.ToUserName;
            chatLog.CreateDate = new DateTimeOffset(DateTime.Now);
            chatLog.ChatGuid = Guid.NewGuid();
            chatLog.Message = request.Message;
            chatLog.FromEmpId = Convert.ToInt32(pkFromEmp);
            chatLog.ToEmPid = Convert.ToInt32(pkToEmp);

        }

        private async Task<int> GetEmpIdFromUserName(string userName)
        {
            var UserDetail = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = userName });
            var empId = Convert.ToInt32(UserDetail.result[0]["pkEmployee"]);
            return empId;
        }

        public async Task<InsertPrivateChatResponse> InsertPrivateChat(InsertPrivateChatRequest request)
        {

            InsertPrivateChatResponse res = new InsertPrivateChatResponse();

            try
            {
                ChatLog chatLog = new ChatLog();
                chatLog.FromUserName = request.FromUser;
                chatLog.ToUserName = request.ToUser;
                chatLog.CreateDate = new DateTimeOffset(DateTime.Now);
                chatLog.ChatGuid = Guid.NewGuid();
                chatLog.Message = request.Message;
                chatLog.FromEmpId = await GetEmpIdFromUserName(request.FromUser);
                chatLog.ToEmPid = await GetEmpIdFromUserName(request.ToUser);
                chatLog.ChatStatusId = request.StatusId;

                var insertRes = _chatLog.Add(chatLog);
                res.id = insertRes.Id;
            }
            catch (Exception ex)
            {
                res.id = null;
                res.Message = ex.Message;
                res.IsSuccess = false;

            }
            return res;

        }

        public PrivateChatUploadFileResponse PrivateChatUploadFile(PrivateChatUploadFileRequest request)
        {
            var res = new PrivateChatUploadFileResponse();

            try
            {
                ChatAttachment attachment = new ChatAttachment
                {
                    FileName = request.FileName,
                    Description = "",
                    FileAddress = request.FilePath,
                    FileType = request.FileExtension,
                    UploadDate = new DateTimeOffset(DateTime.Now),
                    FileSize = request.FileSize,
                    ContentType = request.ContentType,
                    MinioBucket = request.MinioBucket
                };
                var addedChatAttachment = _chatAttachment.Add(attachment);
                res.AttachmentId = addedChatAttachment.Id;
                res.FileName = addedChatAttachment.FileName;

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public GetAttachmentDetailsResponse GetAttachmentDetails(DownloadAttachmentRequest request)
        {
            var res = new GetAttachmentDetailsResponse();
            try
            {


                var foundAttachment = _chatAttachment.GetById(request.AttachmentId);


                //////////////////////////////////////

                if (foundAttachment == null)
                {
                    res.IsSuccess = false;
                    res.Message = "File Not Exists";
                    _logger.LogError($"Id {request.AttachmentId} not found in chatAttachment");
                    return res;

                }

                var foundChatLog = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatAttachments.Where(y => y.ChatAttachmentId == request.AttachmentId)).FirstOrDefault();



                //var foundChatlog = _chatLog.Find(x => x.Id == foundChatLogAttachmet.ChatLogId && x.ToUserName == request.ToUserName.ToLower()).FirstOrDefault();
                if (foundChatLog == null)
                {
                    res.IsSuccess = false;
                    res.Message = $"The file Does not belong to user {request.ToUserName}";
                    _logger.LogError($"Id {request.AttachmentId} not found in chatLogAttachment");
                    return res;
                }

                var filePath = foundAttachment.FileAddress;
                var stream = File.OpenRead(filePath);

                byte[] bytes;
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                string base64 = Convert.ToBase64String(bytes);
                res.FileContent = base64;
                res.FileExtension = foundAttachment.FileType;
                res.FileName = foundAttachment.FileName;

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;





        }


        public async Task<GetAttachmentDetailsResponse> GetAttachmentDetailsTest(DownloadAttachmentRequestTest request)
        {
            var res = new GetAttachmentDetailsResponse();
            try
            {


                var foundAttachment = _chatAttachment.GetById(request.AttachmentId);


                //    //////////////////////////////////////

                if (foundAttachment == null)
                {
                    res.IsSuccess = false;
                    res.Message = "File Not Exists";
                    _logger.LogError($"Id {request.AttachmentId} not found in chatAttachment");
                    return res;

                }

                //    var foundChatLog = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatAttachments.Where(y => y.ChatAttachmentId == request.AttachmentId)).FirstOrDefault();



                //    //var foundChatlog = _chatLog.Find(x => x.Id == foundChatLogAttachmet.ChatLogId && x.ToUserName == request.ToUserName.ToLower()).FirstOrDefault();
                //    if (foundChatLog == null)
                //    {
                //        res.IsSuccess = false;
                //        res.Message = $"The file Does not belong to user {request.ToUserName}";
                //        _logger.LogError($"Id {request.AttachmentId} not found in chatLogAttachment");
                //        return res;
                //    }





                byte[] bytes;

                if (foundAttachment.MinioBucket != null)
                {
                    var minioFileStream = await _minIOService.DownloadFile(foundAttachment.FileName);
                    bytes = minioFileStream.ToArray();

                }
                else
                {
                    var filePath = foundAttachment.FileAddress;
                    var stream = File.OpenRead(filePath);
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
                }

                string base64 = Convert.ToBase64String(bytes);
                res.FileContent = base64;
                res.FileExtension = foundAttachment.FileType;
                res.FileName = foundAttachment.FileName;
                res.FileType = foundAttachment.ContentType;

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;



        }


        public async Task<GetAttachmentDetailsResponseFile> GetAttachmentDetailsTestFile(DownloadAttachmentRequestTest request)
        {
            var res = new GetAttachmentDetailsResponseFile();
            FileStream stream = null;
            try
            {


                var foundAttachment = _chatAttachment.GetById(request.AttachmentId);


                //    //////////////////////////////////////

                //if (foundAttachment == null)
                //{
                //    res.IsSuccess = false;
                //    res.Message = "File Not Exists";
                //    _logger.LogError($"Id {request.AttachmentId} not found in chatAttachment");
                //    return res;

                //}

                //    var foundChatLog = _chatLog.Find(x => x.ToUserName == request.ToUserName).Include(x => x.ChatAttachments.Where(y => y.ChatAttachmentId == request.AttachmentId)).FirstOrDefault();



                //    //var foundChatlog = _chatLog.Find(x => x.Id == foundChatLogAttachmet.ChatLogId && x.ToUserName == request.ToUserName.ToLower()).FirstOrDefault();
                //    if (foundChatLog == null)
                //    {
                //        res.IsSuccess = false;
                //        res.Message = $"The file Does not belong to user {request.ToUserName}";
                //        _logger.LogError($"Id {request.AttachmentId} not found in chatLogAttachment");
                //        return res;
                //    }

                var filePath = foundAttachment.FileAddress;
                var fileName = foundAttachment.FileName;
                res.FileName = fileName;

                if (foundAttachment.MinioBucket != null)
                {
                    res.FileStream = await _minIOService.DownloadFile(foundAttachment.FileName);
                }
                else
                {
                    res.FileStream = File.OpenRead(filePath);
                }
                //stream = File.OpenRead(filePath);

                //byte[] bytes;
                //using (var memoryStream = new MemoryStream())
                //{
                //    stream.CopyTo(memoryStream);
                //    bytes = memoryStream.ToArray();
                //}

                //string base64 = Convert.ToBase64String(bytes);
                //res.FileContent = base64;
                //res.FileExtension = foundAttachment.FileType;
                //res.FileName = foundAttachment.FileName;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return res;





        }

        public async Task<GetAllEmployeeForChatResponse?> GetAllEmployeeForChat(GetAllEmployeeForChatRequest request)
        {
            var foundMaster = _empMaster.Find(x => x.UserName == request.MasterUserName).Include(x => x.UserContancts).Select(x => x.Id).FirstOrDefault();
            var masterContacts = _userContanct.Find(x => x.EmpMasterId == foundMaster).Select(x => x.UserName).ToList();
            masterContacts.Add(request.MasterUserName);
            var masterContactsString = string.Join(',', masterContacts);
            GetEployeeDetailsRequest getEployeeDetailsRequest = new();
            getEployeeDetailsRequest.UserName = masterContactsString;
            var GetEployeeDetailsRes = await _griffinAirAvation.GetEployeeDetails(getEployeeDetailsRequest);
            List<int> foundPks = new();
            foreach (var i in GetEployeeDetailsRes.result)
            {
                foundPks.Add(Convert.ToInt32(i["pkEmployee"]));
            }
            var foundPksStr = string.Join(',', foundPks);
            GetUsersToChatRequest getUsersToChatRequest = new() { pageNumber = request.PageNumber, pageSize = request.PageSize, userName = request.UserName, toUserName = request.ToUserName, notUsersIn = foundPksStr };
            var theRes = await _griffinAirAvation.GetAllEmployeeForChat(getUsersToChatRequest);
            return theRes;

        }

        public async Task<GetAllEmployeeForChatResponse?> GetAllEmployeeForChatV2(GetAllEmployeeForChatRequest request)
        {
            //var foundMaster = _empMaster.Find(x => x.UserName == request.MasterUserName).Include(x => x.UserContancts).Select(x => x.Id).FirstOrDefault();
            //var masterContacts = _userContanct.Find(x => x.EmpMasterId == foundMaster).Select(x => x.UserName).ToList();
            //masterContacts.Add(request.MasterUserName);
            //var masterContactsString = string.Join(',', masterContacts);
            GetEployeeDetailsRequest getEployeeDetailsRequest = new();
            getEployeeDetailsRequest.UserName = request.MasterUserName;
            var GetEployeeDetailsRes = await _griffinAirAvation.GetEployeeDetails(getEployeeDetailsRequest);
            List<int> foundPks = new();
            foreach (var i in GetEployeeDetailsRes.result)
            {
                foundPks.Add(Convert.ToInt32(i["pkEmployee"]));
            }
            var foundPksStr = string.Join(',', foundPks);
            GetUsersToChatRequest getUsersToChatRequest = new() { pageNumber = request.PageNumber, pageSize = request.PageSize, userName = request.UserName, toUserName = request.ToUserName, notUsersIn = foundPksStr };
            var theRes = await _griffinAirAvation.GetAllEmployeeForChat(getUsersToChatRequest);

            foreach (var i in theRes.getAllEmployeeForChatResponseItemViewModels)
            {
                var tmpUserName = i["userName"].ToString();
                var foundMaster = _empMaster.Find(x => x.UserName == tmpUserName).FirstOrDefault();

                if (foundMaster != null && foundMaster.LastSeenDate != null)
                {
                    i.Add("lastSeen", (long)(foundMaster.LastSeenDate.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
                }
                else
                {
                    i.Add("lastSeen", null);
                }
            }
            return theRes;

        }

        public List<Dictionary<string, object>> GetAllRoomMessagesV1(GetAllRoomMessagesV1Request request)
        {
            var resLst = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog })
                 .Where(x => x.chatRoomLog.ChatRoomId == request.ChatRoomId)
                 .Select(x => new Dictionary<string, object> {
                     {"fromUser",x.chatLog.FromUserName},
                     {"toUser",x.chatLog.ToUserName },
                     {"message",x.chatLog.Message },
                     {"roomId",x.chatRoomLog.ChatRoomId }
                 }).ToList();

            //var mm = new { fromUser = x.chatLog.FromUserName, toUser = x.chatLog.ToUserName, message = x.chatLog.Message, roomId = x.chatRoomLog.ChatRoomId };

            return resLst;


        }

        public async Task SaveChatRoomMessage(ChatRoomMessage chatRoomMessage)
        {
            var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == chatRoomMessage.fromChatRoom).FirstOrDefault();
            var chatRoomId = 0L;
            if (foundChatRoom == null)
            {
                _logger.LogError("ChatRoom Not found");
                return;
            }

            if (chatRoomMessage.replyOfGuid == "null")
            {
                chatRoomMessage.replyOfGuid = null;
            }

            chatRoomId = foundChatRoom.Id;
            var fromUserDetails = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = chatRoomMessage.fromUserName });

            if (fromUserDetails.result == null)
            {
                _logger.LogError("User Name not found");
                return;
            }
            var empId = Convert.ToInt32(fromUserDetails.result[0]["pkEmployee"]);
            List<long> chatLogIds = new List<long>();

            foreach (var i in chatRoomMessage.messages)
            {
                if (i.attachmentId != null)
                {
                    var foundAttachment = _chatAttachment.GetById(i.attachmentId);
                    if (foundAttachment == null)
                    {
                        _logger.LogError($"{i.attachmentId} not found for chat room attachment");
                        continue;
                    }
                }

                if (string.IsNullOrEmpty(i.Message))
                {
                    i.Message = " ";
                }
                DateTimeOffset clientDate = DateTimeOffset.FromUnixTimeMilliseconds(i.ClientTime);
                ChatLog chatLog = new ChatLog();
                chatLog.FromEmpId = empId;
                chatLog.Message = i.Message;
                chatLog.IsRtl = i.IsRtl;
                chatLog.CreateDate = DateTimeOffset.Now;
                chatLog.ClientDateTime = clientDate;
                //chatLog.ChatGuid = Guid.NewGuid();
                chatLog.ToUserName = string.Empty;
                chatLog.ToEmPid = -1;
                chatLog.ChatStatusId = 1;
                chatLog.FromUserName = chatRoomMessage.fromUserName;
                chatLog.ChatGuid = i.chatGuid.Value;
                chatLog.ForwardedBy = chatRoomMessage.forwardedBy;
                chatLog.Reply = chatRoomMessage.replyOfGuid;
                _chatLog.Add(chatLog);
                AddAttachmentToMessage(chatLog.Id, i.attachmentId);
                await AddMessageToChatRoomMessage(chatRoomId, chatLog.Id, chatRoomMessage.fromUserName);
            }

        }

        public void AddAttachmentToMessage(long chatlogId, long? attachmentId)
        {
            if (attachmentId != null)
            {

                var foundAttachment = _chatAttachment.GetById(attachmentId);

                if (foundAttachment != null)
                {
                    _chatLogAttachment.Add(new ChatLogAttachment { ChatLogId = chatlogId, ChatAttachmentId = attachmentId.Value });
                }
                else
                {
                    _logger.LogError($"{attachmentId} not found for chat room attachment");
                }
            }


        }

        public async Task AddMessageToChatRoomMessage(long chatRoomId, long chatLogId, string fromUserName)
        {
            //_griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = userName });
            var memberUserNames = _chatRoomMemeber.Find(x => x.ChatRoomId == chatRoomId).Select(x => x.UserName).ToList();
            memberUserNames.Remove(fromUserName);

            int pageNo = 0;
            int PageSize = 50;
            int skip = pageNo * PageSize;

            _chatRoomLog.Add(new ChatRoomLog { ChatLogId = chatLogId, ChatRoomId = chatRoomId });

            while (memberUserNames.Skip(skip).Take(PageSize).Count() > 0)
            {

                var pagedMembers = memberUserNames.Skip(skip).Take(PageSize).ToList();


                string userNames = string.Join(',', pagedMembers);

                var empDetails = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = userNames });

                if (empDetails.success == true)
                {
                    foreach (var i in empDetails.result)
                    {
                        var empId = Convert.ToInt32(i["pkEmployee"]);
                        var empUserName = i["userName"].ToString();
                        UserChatRoomReciever userChatRoomReciever = new UserChatRoomReciever();
                        userChatRoomReciever.UserId = empId;
                        userChatRoomReciever.UserName = empUserName;
                        userChatRoomReciever.ChatStatusId = 1;
                        userChatRoomReciever.ChatLogId = chatLogId;
                        userChatRoomReciever.ChatroomId = chatRoomId;
                        _userChatRoomReciever.Add(userChatRoomReciever);


                    }


                }
                else
                {
                    _logger.LogError("emp Detail in AddMessageToChatRoomMessage encountered a problem AddMessageToChatRoomMessage");
                }
                pageNo++;
                skip = pageNo * PageSize;
            }
        }

        public UsersChatRoomsWithCountOfUnreadsResponseV2 GetUsersChatRoomsWithCountOfUnreadsV2(ListOfRoomsWithUnreadMessagesRequestV2 request)
        {
            int pageNo = (--request.PageNumber);
            int skip = pageNo * request.PageSize;
            UsersChatRoomsWithCountOfUnreadsResponseV2 response = new();
            try
            {
                var roomList = _chatRoomMemeber.Find(x => x.UserName == request.userName).Include(x => x.ChatRoom).Where(x => x.ChatRoom.IsActive == true).Select(x => x.ChatRoom).ToList();
                List<UserChatRoomWithCountOfUnreadsResponseV2> tmpUserChatRoomList = new();

                foreach (var i in roomList)
                {
                    UserChatRoomWithCountOfUnreadsResponseV2 tmpres = new();

                    var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == i.Id).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();
                    //var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == i).Where(x => (x.chatLog.FromUserName == i && x.chatLog.ToUserName == request.ToUserName) || (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i)).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();
                    if (tmpChatLog != null)
                    {
                        tmpres.roomName = i.ChatRoomName;
                        tmpres.lastMessage = tmpChatLog.Message;
                        tmpres.lastMessageDate = tmpChatLog.CreateDate;

                        tmpres.senderUserName = tmpChatLog.FromUserName;
                        tmpres.senderUserId = tmpChatLog.FromEmpId;
                        tmpres.countOfUnreadMessage = _userChatRoomReciever.Find(x => x.ChatroomId == i.Id && x.UserName == request.userName && x.ChatStatusId == 1).Count();
                        tmpUserChatRoomList.Add(tmpres);
                    }
                }
                response.Total = tmpUserChatRoomList.Count();
                response.UserChatRoomsWithCountOfUnreads = tmpUserChatRoomList.Skip(skip).Take(request.PageSize).ToList();


            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public UsersChatRoomsWithCountOfUnreadsResponseV3 GetUsersChatRoomsWithCountOfUnreadsV3(ListOfRoomsWithUnreadMessagesRequestV3 request)
        {
            //int pageNo = (--request.PageNumber);
            //int skip = pageNo * request.PageSize;
            var fromDatetime = DateTimeOffset.FromUnixTimeMilliseconds(request.fromTimeStamp);
            UsersChatRoomsWithCountOfUnreadsResponseV3 response = new();
            try
            {
                var roomList = _chatRoomMemeber.Find(x => x.UserName == request.userName).Include(x => x.ChatRoom).Where(x => x.ChatRoom.IsActive == true).Select(x => x.ChatRoom).ToList();
                List<UserChatRoomWithCountOfUnreadsResponseV3> tmpUserChatRoomList = new();

                foreach (var i in roomList)
                {
                    UserChatRoomWithCountOfUnreadsResponseV3 tmpres = new();

                    var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == i.Id).Where(x => x.chatLog.ClientDateTime > fromDatetime).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();
                    //var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == i).Where(x => (x.chatLog.FromUserName == i && x.chatLog.ToUserName == request.ToUserName) || (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i)).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();
                    if (tmpChatLog != null)
                    {
                        tmpres.roomName = i.ChatRoomName;
                        var roomType = _chatRoomType.GetById(i.ChatRoomTypeId);

                        tmpres.roomType = roomType.IsChannel.Value ? "Channel" : "Group";
                        tmpres.lastMessage = tmpChatLog.Message;
                        tmpres.createDate = tmpChatLog.ClientDateTime.Value.ToUnixTimeMilliseconds();
                        tmpres.userName = request.userName;
                        tmpres.fromUserName = tmpChatLog.FromUserName;
                        tmpres.senderUserId = tmpChatLog.FromEmpId;
                        tmpres.lastMessageId = tmpChatLog.ChatGuid;
                        tmpres.toUserName = request.userName;
                        var foundChatLogAttachment = _chatLogAttachment.Find(x => x.ChatLogId == tmpChatLog.Id).FirstOrDefault();
                        if (foundChatLogAttachment != null)
                        {
                            tmpres.attachment = GetFileDetail(new GetFileDetailRequest { Id = foundChatLogAttachment.ChatAttachmentId });
                        }



                        tmpres.countOfUnreadMessage = _userChatRoomReciever.Find(x => x.ChatroomId == i.Id && x.UserName == request.userName && x.ChatStatusId == 1 && x.ChatLogId < tmpChatLog.Id).Count();
                        tmpUserChatRoomList.Add(tmpres);
                    }
                }
                response.UserChatRoomsWithCountOfUnreads = tmpUserChatRoomList.ToList();
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<UsersChatRoomsWithCountOfUnreadsResponseV4> GetUsersChatRoomsWithCountOfUnreadsV4(ListOfRoomsWithUnreadMessagesRequestV3 request)
        {
            //int pageNo = (--request.PageNumber);
            //int skip = pageNo * request.PageSize;
            var fromDatetime = DateTimeOffset.FromUnixTimeMilliseconds(request.fromTimeStamp);
            UsersChatRoomsWithCountOfUnreadsResponseV4 response = new();
            try
            {
                var roomList = _chatRoomMemeber.Find(x => x.UserName == request.userName).Include(x => x.ChatRoom).Where(x => x.ChatRoom.IsActive == true).Select(x => x.ChatRoom).ToList();
                List<Dictionary<string, object>> tmpUserChatRoomList = new();
                var roomListIds = roomList.Select(x => x.Id).ToList();
                var tmpChatLogList = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => roomListIds.Contains(x.chatRoomLog.ChatRoomId)).Where(x => x.chatLog.ClientDateTime > fromDatetime).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new { x.chatLog, chatRoomId = x.chatRoomLog.ChatRoomId }).ToList();
                var tmpChatLogListIds = tmpChatLogList.Select(x => x.chatLog.Id).ToList();
                var foundChatLogAttachments = _chatLogAttachment.Find(x => tmpChatLogListIds.Contains(x.ChatLogId)).ToList();

                var foundAttachmentIds = foundChatLogAttachments.Select(x => x.ChatAttachmentId).ToList();
                var foundAttachmens = _chatAttachment.Find(x => foundAttachmentIds.Contains(x.Id)).ToList();

                var maxChatLog = tmpChatLogList.Max(x => x.chatLog.Id);

                var userChatRoomRecievers = _userChatRoomReciever.Find(x => roomListIds.Contains(x.ChatroomId) && x.UserName == request.userName && x.ChatStatusId == 1 && x.ChatLogId <= maxChatLog).ToList();
                foreach (var i in roomList)
                {
                    Dictionary<string, object> tmpres = new();

                    //var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == i.Id).Where(x => x.chatLog.ClientDateTime > fromDatetime).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();
                    var tmpChatLog = tmpChatLogList.Where(x => x.chatRoomId == i.Id).Where(x => x.chatLog.ClientDateTime > fromDatetime).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();
                    //var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == i).Where(x => (x.chatLog.FromUserName == i && x.chatLog.ToUserName == request.ToUserName) || (x.chatLog.FromUserName == request.ToUserName && x.chatLog.ToUserName == i)).OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).FirstOrDefault();
                    if (tmpChatLog != null)
                    {
                        tmpres.Add("roomName", i.ChatRoomName);
                        var roomType = _chatRoomType.GetById(i.ChatRoomTypeId);

                        tmpres.Add("roomType", roomType.IsChannel.Value ? "channel" : "group");
                        tmpres.Add("lastMessage", tmpChatLog.Message);
                        tmpres.Add("createDate", tmpChatLog.ClientDateTime.Value.ToUnixTimeMilliseconds());
                        tmpres.Add("userName", request.userName);
                        tmpres.Add("fromUserName", tmpChatLog.FromUserName);
                        tmpres.Add("senderUserId", tmpChatLog.FromEmpId);
                        tmpres.Add("lastMessageId", tmpChatLog.ChatGuid);
                        tmpres.Add("toUserName", request.userName);
                        ///tmpres.Add("msgFromFullName",tmpChatLog.FromEmpId)
                        var foundChatLogAttachment = foundChatLogAttachments.Where(x => x.ChatLogId == tmpChatLog.Id).FirstOrDefault();
                        if (foundChatLogAttachment != null)
                        {
                            var attachment = foundAttachmens.Where(x => x.Id == foundChatLogAttachment.ChatAttachmentId).FirstOrDefault();
                            GetFileDetailResponse getFileDetailResponse = new GetFileDetailResponse
                            {
                                contentType = attachment.ContentType,
                                id = attachment.Id,
                                name = attachment.FileName,
                                size = attachment.FileSize ?? 0,
                                type = attachment.FileType
                            };
                            tmpres.Add("attachment", getFileDetailResponse);
                        }




                        tmpres.Add("countOfUnreadMessage", userChatRoomRecievers.Where(x => x.ChatroomId == i.Id && x.UserName == request.userName && x.ChatStatusId == 1 && x.ChatLogId < tmpChatLog.Id).Count());
                        tmpUserChatRoomList.Add(tmpres);
                    }
                }
                CheckIfAdmin(tmpUserChatRoomList);
                await AddFromFullNameTooRoomLastMessage(tmpUserChatRoomList);
                response.UserChatRoomsWithCountOfUnreads = tmpUserChatRoomList.ToList();
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task AddFromFullNameTooRoomLastMessage(List<Dictionary<string, object>> userChatRoomList)
        {
            List<string> fromUserNames = new();
            foreach (var i in userChatRoomList)
            {
                fromUserNames.Add(i["fromUserName"].ToString().Trim());
            }
            fromUserNames = fromUserNames.Distinct().ToList();
            string commaSeperatedUserNames = string.Join(",", fromUserNames);


            var resDetailFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = commaSeperatedUserNames });
            Dictionary<string, string> resdict = new();
            foreach (var i in resDetailFrom.result)
            {
                resdict.Add(i["userName"].ToString(), i["fullName"].ToString());
            }

            foreach (var i in userChatRoomList)
            {
                //var foundName = await GetFullName(i["fromUserName"].ToString());
                if (resdict.ContainsKey(i["fromUserName"].ToString()))
                {
                    i.Add("fromFullName", resdict[i["fromUserName"].ToString()]);
                }
                else
                {
                    i.Add("fromFullName", "not found");
                }
            }
        }

        public async Task<HistoryChatOfUsersResponse> HistoryChatOfUsers(string toUserName, Dictionary<string, Guid?> request)
        {
            HistoryChatOfUsersResponse response = new();
            try
            {
                var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.Id, y => y.ChatStatus1);
                foreach (var i in request)
                {
                    var fullName = await GetFullName(i.Key);
                    if (i.Value == null)
                    {

                        var foundList = _chatLog.Find(x => ((x.ToUserName == toUserName && x.FromUserName == i.Key) || (x.ToUserName == i.Key && x.FromUserName == toUserName)) && x.IsDeleted != true).OrderByDescending(x => x.ClientDateTime).Take(30).Select(x => new ResponseV2_V2 { createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], toUserName = x.ToUserName, chatRoomName = i.Key }).ToList();
                        response.Result.Add(i.Key, foundList);
                    }
                    else
                    {

                        var foundLog = _chatLog.Find(x => x.ChatGuid == i.Value && x.IsDeleted != true).FirstOrDefault();
                        if (foundLog == null)
                        {
                            response.Warnings.Add($"The {i.Value} not found in database");
                            continue;
                        }
                        //if (foundLog.FromUserName != i.Key)
                        //{
                        //    response.Warnings.Add($"The {i.Value} guid not related to {i.Key}");
                        //    continue;
                        //}
                        var foundList = _chatLog.Find(x => x.ClientDateTime > foundLog.ClientDateTime && ((x.ToUserName == toUserName && x.FromUserName == i.Key) || (x.ToUserName == i.Key && x.FromUserName == toUserName)) && x.IsDeleted != true).OrderByDescending(x => x.ClientDateTime).Select(x => new ResponseV2_V2 { createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], toUserName = x.ToUserName, chatRoomName = i.Key }).ToList();
                        response.Result.Add(i.Key, foundList);
                    }
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return AddFileAttachmentToHistoryChatOfUsers(response);
        }

        public async Task<HistoryChatOfUsersResponseV2> HistoryChatOfUsersV2(string toUserName, Dictionary<string, long> request)
        {
            //HistoryChatOfUsersResponse response = new();
            var listOfUserNames = request.Keys.ToList();
            string commaSeperatedUserNames = string.Join(",", listOfUserNames);


            HistoryChatOfUsersResponseV2 response = new();
            try
            {
                var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.Id, y => y.ChatStatus1);
                var resDetailFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = commaSeperatedUserNames });
                Dictionary<string, string> resdict = new();
                foreach (var i in resDetailFrom.result)
                {
                    resdict.Add(i["userName"].ToString(), i["fullName"].ToString());
                }
                List<string> fromUserNameList = new();

                var userKeys = request.Keys.ToList();
                List<ResponseV2_V2> foundUsersValue0 = new List<ResponseV2_V2>();
                if (request.Count == 0 || request.Values.ToList()[0] == 0)
                {
                    foundUsersValue0 = _chatLog.Find(x => ((x.ToUserName == toUserName && userKeys.Contains(x.FromUserName)) || (userKeys.Contains(x.ToUserName) && x.FromUserName == toUserName)) && x.IsDeleted != true).OrderByDescending(x => x.ClientDateTime).Select(x => new ResponseV2_V2 { forwardedBy = x.ForwardedBy, createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = "fullName", isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], toUserName = x.ToUserName, chatRoomName = "private", replyOf = x.Reply }).ToList();
                }
                else
                {
                    var theDate = request.Values.ToList()[0];
                    var fromDatetime = DateTimeOffset.FromUnixTimeMilliseconds(theDate);
                    //&& x.ClientDateTime > fromDatetime
                    foundUsersValue0 = _chatLog.Find(x => ((x.ToUserName == toUserName && userKeys.Contains(x.FromUserName)) || (userKeys.Contains(x.ToUserName) && x.FromUserName == toUserName)) && x.IsDeleted != true && x.ClientDateTime > fromDatetime).OrderByDescending(x => x.ClientDateTime).Select(x => new ResponseV2_V2 { forwardedBy = x.ForwardedBy, createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = "fullName", isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], toUserName = x.ToUserName, chatRoomName = "private", replyOf = x.Reply }).ToList();

                }

                foreach (var i in request)
                {
                    var fullName = resdict[i.Key];
                    if (i.Value == 0)
                    {

                        var foundList = foundUsersValue0.Where(x => ((x.toUserName == toUserName && x.fromUserName == i.Key) || (x.toUserName == i.Key && x.fromUserName == toUserName))).OrderByDescending(x => x.createDate).Take(50).ToList();
                        foundList.ForEach(x =>
                        {
                            x.fullName = fullName;
                            x.chatRoomName = i.Key;
                        });
                        //response.Result.Add(i.Key, foundList);
                        HistoryMessage tmeHistoryMsg = new() { List = foundList, Total = 50 };
                        var fromUserNamesTmp = foundList.Select(x => x.fromUserName).ToList();
                        fromUserNameList.AddRange(fromUserNamesTmp);
                        response.Result.Add(i.Key, tmeHistoryMsg);
                    }
                    else
                    {
                        var fromDatetime = DateTimeOffset.FromUnixTimeMilliseconds(i.Value);


                        var countOfFoundLog = foundUsersValue0.Where(x => ((x.toUserName == toUserName && x.fromUserName == i.Key) || (x.toUserName == i.Key && x.fromUserName == toUserName))).Count();
                        HistoryMessage tmpHistoryMsg = new() { List = null, Total = countOfFoundLog };
                        if (countOfFoundLog > 50)
                        {
                            var foundListMore = foundUsersValue0.Where(x => ((x.toUserName == toUserName && x.fromUserName == i.Key) || (x.toUserName == i.Key && x.fromUserName == toUserName))).OrderByDescending(x => x.createDate).Take(50).ToList();
                            foundListMore.ForEach(x =>
                            {
                                x.fullName = fullName;
                                x.chatRoomName = i.Key;
                            });
                            tmpHistoryMsg.List = foundListMore;
                        }
                        else
                        {
                            var foundListMore = foundUsersValue0.Where(x => ((x.toUserName == toUserName && x.fromUserName == i.Key) || (x.toUserName == i.Key && x.fromUserName == toUserName)) && x.createDate > i.Value).OrderByDescending(x => x.createDate).ToList();
                            foundListMore.ForEach(x =>
                            {
                                x.fullName = fullName;
                                x.chatRoomName = i.Key;
                            });
                            tmpHistoryMsg.List = foundListMore;
                        }

                        //var foundLog = _chatLog.Find(x => x.ChatGuid == i.Value && x.IsDeleted != true).FirstOrDefault();
                        //if (foundLog == null)
                        //{
                        //    response.Warnings.Add($"The {i.Value} not found in database");
                        //    continue;
                        //}
                        //if (foundLog.FromUserName != i.Key)
                        //{
                        //    response.Warnings.Add($"The {i.Value} guid not related to {i.Key}");
                        //    continue;
                        //}
                        //var foundList = _chatLog.Find(x => x.ClientDateTime > foundLog.ClientDateTime && ((x.ToUserName == toUserName && x.FromUserName == i.Key) || (x.ToUserName == i.Key && x.FromUserName == toUserName)) && x.IsDeleted != true).OrderByDescending(x => x.ClientDateTime).Select(x => new ResponseV2_V2 { createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], toUserName = x.ToUserName, chatRoomName = i.Key }).ToList();
                        //response.Result.Add(i.Key, foundList);
                        var fromUserNamesTmp = tmpHistoryMsg.List.Select(x => x.fromUserName).ToList();
                        fromUserNameList.AddRange(fromUserNamesTmp);
                        response.Result.Add(i.Key, tmpHistoryMsg);
                    }
                }
                fromUserNameList = fromUserNameList.Select(x => x.Trim()).Distinct().ToList();
                var commaSeperatedFromUserNames = string.Join(",", fromUserNameList);
                var resDetailFromUserName = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = commaSeperatedFromUserNames });

                Dictionary<string, string> resdict2 = new();
                foreach (var i in resDetailFromUserName.result)
                {
                    resdict2.Add(i["userName"].ToString(), i["fullName"].ToString());
                }
                foreach (var i in response.Result)
                {
                    var tmpList = i.Value.List;
                    tmpList.ForEach(x => x.msgFromFullName = resdict2[x.fromUserName]);
                }

            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return AddFileAttachmentToHistoryChatOfUsersV2(response);
        }

        public HistoryChatOfUsersResponseV2 AddFileAttachmentToHistoryChatOfUsersV2(HistoryChatOfUsersResponseV2 response)
        {
            var guids = new List<Guid>();
            foreach (var item in response.Result)
            {
                var listOfMessages = item.Value;
                foreach (var message in listOfMessages.List)
                {
                    guids.Add(message.messageId);
                }
            }
            var foundChatLogs = _chatLog.Find(x => guids.Contains(x.ChatGuid)).ToList();
            var foundChatLogIds = foundChatLogs.Select(x => x.Id).ToList();

            var foundChatlogAttachments = _chatLogAttachment.Find(x => foundChatLogIds.Contains(x.ChatLogId)).ToList();
            var foundChatlogAttachmentsIds = foundChatlogAttachments.Select(x => x.ChatAttachmentId).ToList();

            var foundAttachments = _chatAttachment.Find(x => foundChatlogAttachmentsIds.Contains(x.Id)).ToList();

            foreach (var item in response.Result)
            {
                var listOfMessages = item.Value;
                foreach (var message in listOfMessages.List)
                {
                    var foundChatLogId = foundChatLogs.Where(x => x.ChatGuid == message.messageId).FirstOrDefault().Id;
                    var foundChatlogAttachment = foundChatlogAttachments.Where(x => x.ChatLogId == foundChatLogId).FirstOrDefault();
                    if (foundChatlogAttachment != null)
                    {
                        var foundAttachment = foundAttachments.Where(x => x.Id == foundChatlogAttachment.ChatAttachmentId).FirstOrDefault();

                        var foundAttachment1 = GetFileDetail(new GetFileDetailRequest { Id = foundChatlogAttachment.ChatAttachmentId });
                        message.attachment = new GetFileDetailResponse
                        {
                            contentType = foundAttachment.ContentType,
                            id = foundAttachment.Id,
                            name = foundAttachment.FileName,
                            size = foundAttachment.FileSize ?? 0,
                            type = foundAttachment.FileType

                        };

                        //var fileDetail = _chatAttachment.GetById(request.Id);
                        //response.id = fileDetail.Id;
                        //response.name = fileDetail.FileName;
                        //response.size = fileDetail.FileSize ?? 0;
                        //response.type = fileDetail.FileType;
                        //response.contentType = fileDetail.ContentType;
                    }
                }

            }

            return response;
        }

        public HistoryChatOfUsersResponse AddFileAttachmentToHistoryChatOfUsers(HistoryChatOfUsersResponse response)
        {
            foreach (var item in response.Result)
            {
                var listOfMessages = item.Value;
                foreach (var message in listOfMessages)
                {
                    var foundChatLogId = _chatLog.Find(x => x.ChatGuid == message.messageId).FirstOrDefault().Id;
                    var foundChatlogAttachment = _chatLogAttachment.Find(x => x.ChatLogId == foundChatLogId).FirstOrDefault();
                    if (foundChatlogAttachment != null)
                    {
                        var foundAttachment = GetFileDetail(new GetFileDetailRequest { Id = foundChatlogAttachment.ChatAttachmentId });
                        message.attachment = foundAttachment;
                    }
                }

            }
            return response;
        }

        public async Task<string> GetFullName(string userName)
        {
            string fullName = "";
            try
            {
                var resDetailFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = userName });
                fullName = resDetailFrom.result[0]["fullName"].ToString();
            }
            catch (Exception ex)
            {

                fullName = "not found";
                _logger.LogError(ex.StackTrace);
            }


            return fullName;
        }

        public async Task<PrivateChatResponse> PrivateChatService(PrivateChatMessageRequest request)
        {
            PrivateChatResponse res = new();

            var foundChatLog = _chatLog.Find(x => x.FromUserName.ToLower() == request.FromUserName.ToLower() && x.ToUserName.ToLower() == request.ToUserName.ToLower()).OrderByDescending(x => x.Id).Take(10).Select(x => x.Message).ToList();



            if (foundChatLog != null && foundChatLog.Contains(request.Message))
            {

                res.Warnings.Add("This chat log already exists");
                return res;
            }

            res = await _chat.PrivateChat(request);
            return res;
        }

        public async Task<PrivateChatResponse> PrivateChatToUsersService(PrivateChatMessageToUsersRequest request)
        {
            PrivateChatResponse res = new();

            res = await _chat.PrivateChatToUsers(request);
            return res;
        }

        public GetFileDetailResponse GetFileDetail(GetFileDetailRequest request)
        {
            GetFileDetailResponse response = new();
            try
            {
                var fileDetail = _chatAttachment.GetById(request.Id);
                response.id = fileDetail.Id;
                response.name = fileDetail.FileName;
                response.size = fileDetail.FileSize ?? 0;
                response.type = fileDetail.FileType;
                response.contentType = fileDetail.ContentType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }

            return response;


        }

        public UpdateChatRoomMessagesToReadResponse UpdateChatRoomMessagesToRead(UpdateChatRoomMessagesToReadRequest request)
        {
            UpdateChatRoomMessagesToReadResponse response = new();
            try
            {
                var foundRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();

                var foundChats = _userChatRoomReciever.Find(x => x.ChatroomId == foundRoom.Id && x.UserName == request.UserName && x.ChatStatusId == 1).ToList();
                foundChats.ForEach(x =>
                {
                    x.ChatStatusId = 2;
                });

                _context.BulkUpdate(foundChats);
                //foreach (var i in foundChats)
                //{
                //    i.ChatStatusId = 2;
                //    _userChatRoomReciever.Update(i);
                //}
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.StackTrace;
            }
            return response;
        }


        public async Task<UserRoomHistoryResponse> UserRoomHistory(UserRoomHistoryRequest request)
        {


            var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.Id, y => y.ChatStatus1);
            UserRoomHistoryResponse response = new();
            var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();
            if (foundChatRoom == null)
            {
                response.IsSuccess = false;
                response.Message = "chat room not found";
                return response;
            }

            var founChatRoomType = _chatRoomType.Find(x => x.Id == foundChatRoom.ChatRoomTypeId).FirstOrDefault();
            var chatRoomType = founChatRoomType.IsChannel == true ? "channel" : "group";

            var foundMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName == request.UserName).FirstOrDefault();
            if (foundMember == null)
            {
                response.IsSuccess = false;
                response.Message = "user not member of room";
                return response;
            }
            List<ResponseV2_V2> tmpChatLog = new();
            if (request.Chatguid != null)
            {

                var foundChatWithGuid = (from chatlog in _context.ChatLogs
                                         join chatRoomLog in _context.ChatRoomLogs
                                         on chatlog.Id equals chatRoomLog.ChatLogId
                                         where chatlog.ChatGuid == request.Chatguid && chatRoomLog.ChatRoomId == foundChatRoom.Id
                                         select chatlog).FirstOrDefault();
                if (foundChatWithGuid == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Guid does not belong to Room";
                    return response;
                }

                tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id && x.chatLog.ClientDateTime > foundChatWithGuid.ClientDateTime).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();
            }
            else
            {
                tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();

            }

            //var tm2 = from chatlog in _context.ChatLogs
            //          join chatroomlog in _context.ChatRoomLogs
            //          on chatlog.Id equals chatroomlog.ChatLogId
            //          where chatroomlog.ChatRoomId== foundChatRoom.Id
            //          select chatlog;
            //var tm3 = tm2.ToList();
            foreach (var i in tmpChatLog)
            {
                i.fullName = await GetFullName(i.fromUserName);
            }

            //{ createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], toUserName = x.ToUserName }
            response.roomHistories = tmpChatLog;
            AddFileAttachmentToUserRoomHistory(response);
            return response;

        }

        public async Task<UserRoomHistoryResponse> UserRoomHistoryV2(UserRoomHistoryRequestV3 request)
        {


            var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.Id, y => y.ChatStatus1);
            UserRoomHistoryResponse response = new();
            var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();
            if (foundChatRoom == null)
            {
                response.IsSuccess = false;
                response.Message = "chat room not found";
                return response;
            }

            var founChatRoomType = _chatRoomType.Find(x => x.Id == foundChatRoom.ChatRoomTypeId).FirstOrDefault();
            var chatRoomType = founChatRoomType.IsChannel == true ? "channel" : "group";

            var foundMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName == request.UserName).FirstOrDefault();
            if (foundMember == null)
            {
                response.IsSuccess = false;
                response.Message = "user not member of room";
                return response;
            }
            List<ResponseV2_V2> tmpChatLog = new();
            if (request.Chatguid != null)
            {

                var foundChatWithGuid = (from chatlog in _context.ChatLogs
                                         join chatRoomLog in _context.ChatRoomLogs
                                         on chatlog.Id equals chatRoomLog.ChatLogId
                                         where chatlog.ChatGuid == request.Chatguid && chatRoomLog.ChatRoomId == foundChatRoom.Id
                                         select chatlog).FirstOrDefault();
                if (foundChatWithGuid == null || foundChatWithGuid.IsDeleted == true)
                {
                    response.IsSuccess = false;
                    response.Message = "Guid does not belong to Room";
                    return response;
                }

                if (request.isForward == true)
                {
                    if (request.isIncluded == false)
                    {
                        tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id && x.chatLog.ClientDateTime > foundChatWithGuid.ClientDateTime && x.chatLog.IsDeleted != true).OrderByDescending(x => x.chatLog.CreateDate).Take(request.perPage).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();
                    }
                    else
                    {
                        tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id && x.chatLog.ClientDateTime >= foundChatWithGuid.ClientDateTime && x.chatLog.IsDeleted != true).OrderByDescending(x => x.chatLog.CreateDate).Take(request.perPage).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();

                    }
                }
                else
                {
                    if (request.isIncluded == false)
                    {
                        tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id && x.chatLog.ClientDateTime < foundChatWithGuid.ClientDateTime && x.chatLog.IsDeleted != true).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();
                    }
                    else
                    {
                        tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id && x.chatLog.ClientDateTime <= foundChatWithGuid.ClientDateTime && x.chatLog.IsDeleted != true).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();

                    }

                }
            }
            else
            {
                tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id && x.chatLog.IsDeleted != true).OrderByDescending(x => x.chatLog.CreateDate).Take(request.perPage).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();

            }

            //var tm2 = from chatlog in _context.ChatLogs
            //          join chatroomlog in _context.ChatRoomLogs
            //          on chatlog.Id equals chatroomlog.ChatLogId
            //          where chatroomlog.ChatRoomId== foundChatRoom.Id
            //          select chatlog;
            //var tm3 = tm2.ToList();
            foreach (var i in tmpChatLog)
            {
                i.fullName = await GetFullName(i.fromUserName);
            }

            //{ createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], toUserName = x.ToUserName }
            response.roomHistories = tmpChatLog;
            AddFileAttachmentToUserRoomHistory(response);
            return response;

        }
        public async Task<HistoryMessage> UserRoomHistoryV2(UserRoomHistoryRequestV2 request)
        {


            var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.Id, y => y.ChatStatus1);
            //UserRoomHistoryResponse response = new();
            UserRoomHistoryResponseV2 response = new();
            var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.ChatRoomName).FirstOrDefault();
            //if (foundChatRoom == null)
            //{
            //    response.IsSuccess = false;
            //    response.Message = "chat room not found";
            //    return response;
            //}

            var founChatRoomType = _chatRoomType.Find(x => x.Id == foundChatRoom.ChatRoomTypeId).FirstOrDefault();
            var chatRoomType = founChatRoomType.IsChannel == true ? "channel" : "group";

            //var foundMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName == request.UserName).FirstOrDefault();
            //if (foundMember == null)
            //{
            //    response.IsSuccess = false;
            //    response.Message = "user not member of room";
            //    return response;
            //}
            List<ResponseV2_V2> tmpChatLog = new();
            HistoryMessage historyMessage = new HistoryMessage();
            if (request.TimeStamp != 0)
            {
                var fromDatetime = DateTimeOffset.FromUnixTimeMilliseconds(request.TimeStamp);

                var foundChatWithGuidCount = (from chatlog in _context.ChatLogs
                                              join chatRoomLog in _context.ChatRoomLogs
                                              on chatlog.Id equals chatRoomLog.ChatLogId
                                              where chatRoomLog.ChatRoomId == foundChatRoom.Id && chatlog.ClientDateTime > fromDatetime
                                              select chatlog).Count();
                historyMessage.Total = foundChatWithGuidCount;


                if (foundChatWithGuidCount > 50)
                {
                    tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id && x.chatLog.ClientDateTime > fromDatetime).OrderByDescending(x => x.chatLog.CreateDate).Take(50).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();

                }
                else
                {
                    tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id && x.chatLog.ClientDateTime > fromDatetime).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();
                }
                historyMessage.List = tmpChatLog;


            }
            else
            {
                historyMessage.Total = 50;
                tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == foundChatRoom.Id).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new RoomHistory().GetObjFromChatLogWithRoomType(x.chatLog, request.ChatRoomName, chatRoomType, chatStatusDict)).ToList();
                historyMessage.List = tmpChatLog;

            }

            //var tm2 = from chatlog in _context.ChatLogs
            //          join chatroomlog in _context.ChatRoomLogs
            //          on chatlog.Id equals chatroomlog.ChatLogId
            //          where chatroomlog.ChatRoomId== foundChatRoom.Id
            //          select chatlog;
            //var tm3 = tm2.ToList();
            var fromUserNames = tmpChatLog.Select(x => x.fromUserName).Distinct().ToList();
            string commaSeperatedUserNames = string.Join(",", fromUserNames);

            var resDetailFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = commaSeperatedUserNames });
            Dictionary<string, string> resdict = new();
            foreach (var i in resDetailFrom.result)
            {
                resdict.Add(i["userName"].ToString(), i["fullName"].ToString());
            }

            foreach (var i in tmpChatLog)
            {
                //i.fullName = await GetFullName(i.fromUserName);
                i.fullName = resdict[i.fromUserName];
            }

            //{ createDate = x.ClientDateTime.Value.ToUnixTimeMilliseconds(), fromUserName = x.FromUserName, fullName = fullName, isRtl = x.IsRtl, lastMessage = x.Message, messageId = x.ChatGuid, pkEmployee = x.FromEmpId, status = chatStatusDict[x.ChatStatusId], toUserName = x.ToUserName }

            AddFileAttachmentToUserRoomHistoryV2(historyMessage);
            return historyMessage;

        }

        public void AddFileAttachmentToUserRoomHistoryV2(HistoryMessage chats)
        {
            var chatguids = chats.List.Select(x => x.messageId).ToList();
            var foundChatlogs = _chatLog.Find(x => chatguids.Contains(x.ChatGuid)).ToList();
            var foundChatLogIds = foundChatlogs.Select(x => x.Id).ToList();
            var chatLogAttachments = _chatLogAttachment.Find(x => foundChatLogIds.Contains(x.ChatLogId)).ToList();
            var chatAttachmentIds = chatLogAttachments.Select(x => x.ChatAttachmentId).ToList();
            var foundAttachments = _chatAttachment.Find(x => chatAttachmentIds.Contains(x.Id)).ToList();

            foreach (var i in chats.List)
            {

                var foundChatLog = foundChatlogs.Where(x => x.ChatGuid == i.messageId).FirstOrDefault();
                var chatLogAttachment = chatLogAttachments.Where(x => x.ChatLogId == foundChatLog.Id).FirstOrDefault();
                if (chatLogAttachment != null)
                {
                    var foundAttachment = foundAttachments.Where(x => x.Id == chatLogAttachment.ChatAttachmentId).FirstOrDefault();
                    //var fileDetail = GetFileDetail(new GetFileDetailRequest { Id = foundAttachment.Id });
                    i.attachment = new GetFileDetailResponse
                    {
                        contentType = foundAttachment.ContentType,
                        type = foundAttachment.FileType,
                        id = foundAttachment.Id,
                        name = foundAttachment.FileName,
                        size = foundAttachment.FileSize ?? 0
                    };
                }

            }

        }

        public void AddFileAttachmentToUserRoomHistory(UserRoomHistoryResponse chats)
        {
            foreach (var i in chats.roomHistories)
            {

                var foundChatLog = _chatLog.Find(x => x.ChatGuid == i.messageId).FirstOrDefault();
                var chatLogAttachment = _chatLogAttachment.Find(x => x.ChatLogId == foundChatLog.Id).FirstOrDefault();
                if (chatLogAttachment != null)
                {
                    var foundAttachment = _chatAttachment.Find(x => x.Id == chatLogAttachment.ChatAttachmentId).FirstOrDefault();
                    var fileDetail = GetFileDetail(new GetFileDetailRequest { Id = foundAttachment.Id });
                    i.attachment = fileDetail;
                }

            }

        }



        public async Task<RoomMembersResponse> RoomMembers(RoomMembersRequest request)
        {
            RoomMembersResponse response = new();

            try
            {
                int pageNo = (--request.RoomDetail.PageNumber);
                int skip = pageNo * request.RoomDetail.PageSize;

                var checkMemberResult = _adminService.CheckIfUserIsRoomMember(request.UserName, request.RoomDetail.RoomName);

                if (checkMemberResult.IsRoomMember == false)
                {
                    response.IsSuccess = false;
                    response.Message = checkMemberResult.Message;
                    return response;

                }

                var roommembers = _chatRoomMemeber.Find(x => x.ChatRoomId == checkMemberResult.RoomId).Select(x => new RoomMemberDetail { isAdmin = x.IsAdmin ?? false, userName = x.UserName }).Skip(skip).Take(request.RoomDetail.PageSize).ToList();
                var countOfMembers = _chatRoomMemeber.Find(x => x.ChatRoomId == checkMemberResult.RoomId).Count();
                var roomUserNames = roommembers.Select(x => x.userName).Distinct().ToList();
                var roomUserNamesCommaSeperated = string.Join(",", roomUserNames);

                var fromUserNameDetails = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = roomUserNamesCommaSeperated });

                Dictionary<string, string[]> resdict = new();

                foreach (var i in fromUserNameDetails.result)
                {
                    var pkEmployee = i["pkEmployee"].ToString();
                    var fullName = i["fullName"].ToString();

                    resdict.Add(i["userName"].ToString(), [pkEmployee, fullName]);
                }


                roommembers.ForEach(x =>
                {
                    if (resdict.ContainsKey(x.userName))
                    {
                        x.pkEmployee = resdict[x.userName][0];
                        x.fullName = resdict[x.userName][1];
                    }
                    var foundEmp = _empMaster.Find(y => y.UserName == x.userName).FirstOrDefault();

                    if (foundEmp != null)
                    {
                        if (foundEmp.LastSeenDate != null)
                        {
                            x.lastSeen = new DateTimeOffset(foundEmp.LastSeenDate.Value).ToUnixTimeMilliseconds();
                        }
                    }

                });

                response.roomMemberDetails = roommembers;
                response.TotalCount = countOfMembers;
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }


        public async Task<RoomMembersResponse> RoomMembersWithNotif(RoomMembersRequest request)
        {
            RoomMembersResponse response = new();

            int pageNo = (--request.RoomDetail.PageNumber);
            int skip = pageNo * request.RoomDetail.PageSize;

            try
            {
                var checkMemberResult = _adminService.CheckIfUserIsRoomMember(request.UserName, request.RoomDetail.RoomName);

                if (checkMemberResult.IsRoomMember == false)
                {
                    response.IsSuccess = false;
                    response.Message = checkMemberResult.Message;
                    return response;

                }

                var roommembers = _chatRoomMemeber.Find(x => x.ChatRoomId == checkMemberResult.RoomId && x.CantGetNotif != true).Select(x => new RoomMemberDetail { isAdmin = x.IsAdmin ?? false, userName = x.UserName }).Skip(skip).Take(request.RoomDetail.PageSize).ToList();
                var countOfMembers = _chatRoomMemeber.Find(x => x.ChatRoomId == checkMemberResult.RoomId).Count();
                var roomUserNames = roommembers.Select(x => x.userName).Distinct().ToList();


                Dictionary<string, string[]> resdict = new();

                for (int i = 0; i < roomUserNames.Count; i++)
                {
                    int tmpSkip = i * 50;
                    var tmpRoomUserName = roomUserNames.Skip(tmpSkip).Take(50).ToList();

                    if (tmpRoomUserName == null || tmpRoomUserName.Count == 0)
                    {
                        break;
                    }

                    var roomUserNamesCommaSeperated1 = string.Join(",", tmpRoomUserName);
                    var fromUserNameDetails1 = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = roomUserNamesCommaSeperated1 });
                    foreach (var j in fromUserNameDetails1.result)
                    {
                        var pkEmployee = j["pkEmployee"].ToString();
                        var fullName = j["fullName"].ToString();

                        if (!resdict.ContainsKey(j["userName"].ToString()))
                        {
                            resdict.Add(j["userName"].ToString(), [pkEmployee, fullName]);
                        }
                    }

                }

                //var roomUserNamesCommaSeperated = string.Join(",", roomUserNames);



                //var fromUserNameDetails = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = roomUserNamesCommaSeperated });



                //foreach (var i in fromUserNameDetails.result)
                //{
                //    var pkEmployee = i["pkEmployee"].ToString();
                //    var fullName = i["fullName"].ToString();

                //    resdict.Add(i["userName"].ToString(), [pkEmployee, fullName]);
                //}


                roommembers.ForEach(x =>
                {
                    if (resdict.ContainsKey(x.userName))
                    {
                        x.pkEmployee = resdict[x.userName][0];
                        x.fullName = resdict[x.userName][1];
                    }
                    //var foundEmp = _empMaster.Find(y => y.UserName == x.userName).FirstOrDefault();

                    //if (foundEmp != null)
                    //{
                    //    if (foundEmp.LastSeenDate != null)
                    //    {
                    //        x.lastSeen = new DateTimeOffset(foundEmp.LastSeenDate.Value).ToUnixTimeMilliseconds();
                    //    }
                    //}


                });

                var mm = resdict.Keys.ToList();
                var emps = _empMaster.Find(x => mm.Contains(x.UserName)).ToList();
                roommembers.ForEach(x =>
                {

                    var foundEmp = emps.Where(y => y.UserName == x.userName).FirstOrDefault();
                    if (foundEmp != null)
                    {
                        if (foundEmp.LastSeenDate != null)
                        {
                            x.lastSeen = new DateTimeOffset(foundEmp.LastSeenDate.Value).ToUnixTimeMilliseconds();
                        }
                    }

                });



                response.roomMemberDetails = roommembers;
                response.TotalCount = roommembers.Count;
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<RoomMembersResponse> RoomMembersWithNotifV2(RoomMembersRequestV2 request)
        {
            RoomMembersResponse response = new();

            int pageNo = (--request.RoomDetail.PageNumber);
            int skip = pageNo * request.RoomDetail.PageSize;

            try
            {
                var checkMemberResult = _adminService.CheckIfUserIsRoomMemberV2(request.UserName, request.RoomDetail.RoomId);

                if (checkMemberResult.IsRoomMember == false)
                {
                    response.IsSuccess = false;
                    response.Message = checkMemberResult.Message;
                    return response;

                }

                var roommembers = _chatRoomMemeber.Find(x => x.ChatRoomId == checkMemberResult.RoomId && x.CantGetNotif != true).Select(x => new RoomMemberDetail { isAdmin = x.IsAdmin ?? false, userName = x.UserName }).Skip(skip).Take(request.RoomDetail.PageSize).ToList();
                var countOfMembers = _chatRoomMemeber.Find(x => x.ChatRoomId == checkMemberResult.RoomId).Count();
                var roomUserNames = roommembers.Select(x => x.userName).Distinct().ToList();


                Dictionary<string, string[]> resdict = new();

                for (int i = 0; i < roomUserNames.Count; i++)
                {
                    int tmpSkip = i * 50;
                    var tmpRoomUserName = roomUserNames.Skip(tmpSkip).Take(50).ToList();

                    if (tmpRoomUserName == null || tmpRoomUserName.Count == 0)
                    {
                        break;
                    }

                    var roomUserNamesCommaSeperated1 = string.Join(",", tmpRoomUserName);
                    var fromUserNameDetails1 = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = roomUserNamesCommaSeperated1 });
                    foreach (var j in fromUserNameDetails1.result)
                    {
                        var pkEmployee = j["pkEmployee"].ToString();
                        var fullName = j["fullName"].ToString();

                        if (!resdict.ContainsKey(j["userName"].ToString()))
                        {
                            resdict.Add(j["userName"].ToString(), [pkEmployee, fullName]);
                        }
                    }

                }

                //var roomUserNamesCommaSeperated = string.Join(",", roomUserNames);



                //var fromUserNameDetails = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = roomUserNamesCommaSeperated });



                //foreach (var i in fromUserNameDetails.result)
                //{
                //    var pkEmployee = i["pkEmployee"].ToString();
                //    var fullName = i["fullName"].ToString();

                //    resdict.Add(i["userName"].ToString(), [pkEmployee, fullName]);
                //}


                roommembers.ForEach(x =>
                {
                    if (resdict.ContainsKey(x.userName))
                    {
                        x.pkEmployee = resdict[x.userName][0];
                        x.fullName = resdict[x.userName][1];
                    }
                    //var foundEmp = _empMaster.Find(y => y.UserName == x.userName).FirstOrDefault();

                    //if (foundEmp != null)
                    //{
                    //    if (foundEmp.LastSeenDate != null)
                    //    {
                    //        x.lastSeen = new DateTimeOffset(foundEmp.LastSeenDate.Value).ToUnixTimeMilliseconds();
                    //    }
                    //}


                });

                var mm = resdict.Keys.ToList();
                var emps = _empMaster.Find(x => mm.Contains(x.UserName)).ToList();
                roommembers.ForEach(x =>
                {

                    var foundEmp = emps.Where(y => y.UserName == x.userName).FirstOrDefault();
                    if (foundEmp != null)
                    {
                        if (foundEmp.LastSeenDate != null)
                        {
                            x.lastSeen = new DateTimeOffset(foundEmp.LastSeenDate.Value).ToUnixTimeMilliseconds();
                        }
                    }

                });



                response.roomMemberDetails = roommembers;
                response.TotalCount = roommembers.Count;
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }





        public async Task<HistoryChatOfUsersResponse> HistoryChatOfUsersV2(HistoryChatOfUsersV2Request request)
        {
            Dictionary<string, Guid?> privateChatRequest = new Dictionary<string, Guid?>();

            foreach (var i in request.RquestDetails)
            {
                if (i.type == "private")
                {
                    privateChatRequest.Add(i.name, i.lastMessageId);
                }
            }

            HistoryChatOfUsersResponse privateHistoryResponse = await HistoryChatOfUsers(request.UserName, privateChatRequest);

            foreach (var i in request.RquestDetails)
            {
                if (i.type == "room")
                {
                    var roomHistoryResult = await UserRoomHistory(new UserRoomHistoryRequest { UserName = request.UserName, Chatguid = i.lastMessageId, ChatRoomName = i.name });
                    privateHistoryResponse.Result.Add(i.name, roomHistoryResult.roomHistories);// = roomHistoryResult.roomHistories;
                }
            }

            return privateHistoryResponse;

        }

        public async Task<HistoryChatOfUsersResponseV2> HistoryChatOfUsersPooling(HistoryChatOfUsersPoolingRequest request)
        {
            Dictionary<string, long> privateChatRequest = new Dictionary<string, long>();

            foreach (var i in request.RquestDetails)
            {
                if (i.type == "private")
                {
                    privateChatRequest.Add(i.name, i.TimeStamp);
                }
            }

            HistoryChatOfUsersResponseV2 privateHistoryResponse = await HistoryChatOfUsersV2(request.UserName, privateChatRequest);

            foreach (var i in request.RquestDetails)
            {
                if (i.type != "private")
                {
                    var roomHistoryResult = await UserRoomHistoryV2(new UserRoomHistoryRequestV2 { UserName = request.UserName, TimeStamp = i.TimeStamp, ChatRoomName = i.roomName });
                    privateHistoryResponse.Result.Add(i.roomName, roomHistoryResult);// = roomHistoryResult.roomHistories;
                }
            }

            return privateHistoryResponse;

        }

        public DeletePrivateChatResponse DeletePrivateChat(DeletePrivateChatServiceRequest request)
        {
            DeletePrivateChatResponse response = new DeletePrivateChatResponse();

            try
            {
                var foundChatsToDelete = _chatLog.Find(x => request.PrivateChatIds.Contains(x.ChatGuid)).ToList();

                foreach (var i in foundChatsToDelete)
                {
                    i.IsDeleted = true;
                    _chatLog.Update(i);
                    response.DeletedMessageIds.Add(i.ChatGuid);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }



        public AddUsersToRoomResponse DeleteUserFromRoom(DeleteUserFromRoomRequest request)
        {
            AddUsersToRoomResponse res = new();
            var foundRoom = _chatRoom.Find(x => x.ChatRoomName == request.RoomName).FirstOrDefault();


            if (foundRoom == null)
            {
                res.IsSuccess = false;
                res.Message = "Room Not found";
                return res;
            }

            var foundRequestorMember = _chatRoomMemeber.Find(x => x.UserName == request.Requestor && x.ChatRoomId == foundRoom.Id).FirstOrDefault();
            if (foundRequestorMember == null)
            {
                res.IsSuccess = false;
                res.Message = "Requestor is not the member of room";
                return res;
            }

            if (foundRequestorMember.IsAdmin != true)
            {
                res.IsSuccess = false;
                res.Message = "Requestor must be at least admin";
                return res;
            }

            var requestUserNames = request.UserNames.Distinct().ToList();
            var foundMember = _chatRoomMemeber.Find(x => requestUserNames.Contains(x.UserName) && x.ChatRoomId == foundRoom.Id).ToList();

            var foundAdminMember = foundMember.Where(x => x.IsAdmin == true).ToList();

            if (foundAdminMember != null)
            {
                if (foundRoom.CreatorUserName != request.Requestor)
                {
                    res.IsSuccess = false;
                    res.Message = "To delete an admin from room the requestor must be the owner";
                    return res;
                }
            }


            var foundMemberUserNames = foundMember.Select(x => x.UserName);
            if (foundMemberUserNames.Count() != requestUserNames.Count())
            {
                var diffList = requestUserNames.Except(foundMemberUserNames).ToList();
                var diffListStr = string.Join(",", diffList);
                res.Warnings.Add($"{diffListStr} not members of room");
            }





            return res;
            //try
            //{
            //    var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.RoomName).FirstOrDefault();
            //    if (foundChatRoom == null)
            //    {
            //        res.Message = "Room Not found";
            //        res.IsSuccess = false;
            //        return res;
            //    }

            //    var roomType = _chatRoomType.GetById(foundChatRoom.ChatRoomTypeId);

            //    if (roomType.IsChannel.Value)
            //    {
            //        var adminMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName == request.requester && x.IsAdmin == true).FirstOrDefault();
            //        if (adminMember == null)
            //        {
            //            res.Message = "requestor is not an admin";
            //            res.IsSuccess = false;
            //            return res;
            //        }
            //    }

            //    var userNames = request.InviteeUserNames.Split(",").Select(x => x.Trim()).Distinct().ToList();
            //    //var userNames = new List<CreateRGUser>();
            //    //userNames.Remove(request.InviteeUserNames);

            //    var foundMember = _chatRoomMemeber.Find(x => userNames.Contains(x.UserName) && x.ChatRoomId == foundChatRoom.Id).ToList();

            //    _chatRoomMemeber.RemoveRange(foundMember);

            //    return res;
            //}
            //catch (Exception ex)
            //{

            //    res.IsSuccess = false;
            //    res.Message = ex.Message;
            //    return res;
            //}
        }

        public AddUsersToRoomResponse MakeUsersAdminOrNot(MakeUsersAdminOrNotRequest request)
        {
            AddUsersToRoomResponse res = new();
            try
            {
                var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.RoomName).FirstOrDefault();
                if (foundChatRoom == null)
                {
                    res.Message = "Room Not found";
                    res.IsSuccess = false;
                    return res;
                }


                var foundCreator = _chatRoom.Find(x => x.CreatorUserName == request.requester && x.Id == foundChatRoom.Id).FirstOrDefault();
                //var adminMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName == request.requester && x.IsAdmin == true).FirstOrDefault();
                if (foundCreator == null)
                {
                    res.Message = "requestor is not an admin";
                    res.IsSuccess = false;
                    return res;
                }



                //var userNames = request.InviteeUserNames.Split(",").Select(x => x.Trim()).Distinct().ToList();
                var userNames = request.InviteeUserNames.Select(x => x.UserName).Distinct().ToList();


                var foundMember = _chatRoomMemeber.Find(x => userNames.Contains(x.UserName) && x.ChatRoomId == foundChatRoom.Id).ToList();
                foreach (var i in foundMember)
                {
                    var theUser = request.InviteeUserNames.Where(x => x.UserName == i.UserName).FirstOrDefault();
                    if (theUser.UserName == request.requester)
                    {
                        res.Warnings.Add($"{request.requester} is the owner.The owner must be the admin");
                        continue;
                    }
                    i.IsAdmin = theUser.IsAdmin;
                }
                _chatRoomMemeber.UpdateRange(foundMember);

                if (!string.IsNullOrWhiteSpace(request.NewOwoner))
                {
                    var memberForOwner = foundMember.Where(x => x.UserName == request.NewOwoner).FirstOrDefault();
                    if (memberForOwner == null)
                    {
                        res.Warnings.Add($"{request.NewOwoner} is not member of this room");
                    }
                    else
                    {
                        foundCreator.CreatorUserName = request.NewOwoner;
                        _chatRoom.Update(foundCreator);
                        memberForOwner.IsAdmin = true;
                        _chatRoomMemeber.Update(memberForOwner);
                    }
                }


                //foundMember.ForEach(x => x.IsAdmin = request.IsAdmin);


                return res;
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;
                return res;
            }

        }

        public CheckOfflineActionsResponse CheckOfflineActions(string userName)
        {

            CheckOfflineActionsResponse res = new();

            try
            {
                var allActionTypes = _actionType.GetAll().ToDictionary(i => i.Id, j => j.Name);
                var foundActions = _offLineAction.Find(x => x.Done == false && x.ToUserName == userName).Select(x => x.Action).ToList();
                res.Result = foundActions;
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public ResponseMessage InstertToOffLineAction(InstertToOffLineActionRequest request)
        {
            ResponseMessage res = new();

            try
            {
                if (request.IsOnline == false && request.ActionTypeId != 3)
                {
                    OfflineAction offlineAction = new()
                    {
                        Action = request.Action,
                        ActionTypeId = request.ActionTypeId,
                        CreateDateTime = DateTime.Now,
                        Done = false,
                        FromUseName = request.FromUserName,
                        ToUserName = request.ToUserName,
                        DoneDateTime = null

                    };
                    _offLineAction.Add(offlineAction);
                }
                PerformPayload(request.Action, request.ActionTypeId);


            }
            catch (Exception ex)
            {

                res.Message = ex.Message;
                res.IsSuccess = false;
            }
            return res;
        }

        public void PerformPayload(string parameters, long typeId)
        {
            var foundType = _actionType.GetById(typeId);

            Type type = Type.GetType($"ChatV1.Service.Model.{foundType.Name}");
            //var insts= (IPayLoad) Activator.CreateInstance(type);
            var insts = (IPayLoad)ActivatorUtilities.CreateInstance(_provider, type);
            insts.Perform(parameters);
        }

        public ResponseMessage UpdateOfflineActionByType(UpdateOfflineActionByTypeRequest request)
        {

            ResponseMessage res = new();
            try
            {
                var foundActions = _offLineAction.Find(x => x.ToUserName == request.ToUserName && x.FromUseName == request.FromUserName && x.ActionTypeId == x.ActionTypeId).ToList();
                foundActions.ForEach(x =>
                {
                    x.Done = true;
                    x.DoneDateTime = DateTime.Now;
                });
                _offLineAction.UpdateRange(foundActions);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
                _logger.LogError(ex.Message);
            }
            return res;
        }

        public ResponseMessage UpdateMessageToReadFromTo(UpdateMessageToReadFromToRequest request)
        {
            ResponseMessage res = new();
            var statusDict = _chatStatus.Find(x => true).ToDictionary(j => j.ChatStatus1.ToLower(), k => k.Id);
            var found = _chatLog.Find(x => x.FromUserName == request.FromUserName && x.ToUserName == request.ToUserName && x.ChatStatusId == statusDict["sent"]).ToList();

            found.ForEach(x => x.ChatStatusId = statusDict["seen"]);

            _context.BulkUpdate(found);

            return res;
        }


        public ResponseMessage DeleteChatOffline(DeleteChatOfflineRequest request)
        {
            ResponseMessage res = new();
            try
            {
                var foundMessage = _chatLog.Find(x => x.FromUserName == request.FromUsername && x.ToUserName == request.Username && request.MessageGuid == request.MessageGuid).FirstOrDefault();
                foundMessage.IsDeleted = true;
                _chatLog.Update(foundMessage);
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;

        }

        public ResponseMessage MakeOfflineActionDone(MakeOfflineActionDoneRequest request)
        {
            ResponseMessage res = new();
            try
            {
                var foundOfflines = _offLineAction.Find(x => x.ToUserName == request.UserName && x.Done == false).ToList();
                foundOfflines.ForEach(x => x.Done = true);
                _context.BulkUpdate(foundOfflines);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public ResponseMessage MakeMessageReadBeforeGuid(MakeMessageReadBeforeGuidRequest request)
        {
            ResponseMessage res = new();
            try
            {
                var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.ChatStatus1, x => x.Id);
                var foundChatLog = _chatLog.Find(x => x.ChatGuid == request.MessageGuid && x.FromUserName == request.FromUserName && x.ToUserName == request.ToUserName).FirstOrDefault();
                var listOfBefore = _chatLog.Find(x => x.ClientDateTime >= foundChatLog.ClientDateTime && x.FromUserName == request.FromUserName && x.ToUserName == request.ToUserName).ToList();
                listOfBefore.ForEach(x => x.ChatStatusId = chatStatusDict["seen"]);
                _context.BulkUpdate(listOfBefore);
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;
            }
            return res;

        }

        public async Task<SendMessaageToRoomResponse> SendMessageToFlighChannel(SendMessageToFlighChannelRequest request)
        {
            SendMessaageToRoomResponse roomMessageResponse = new();

            var foundMessages = _chatLog.Find(x => x.FromUserName == "griffin" && string.IsNullOrEmpty(x.ToUserName)).OrderByDescending(x => x.Id).Take(10).Select(x => x.Message).ToList();

            if (foundMessages != null && foundMessages.Contains(request.message))
            {
                roomMessageResponse.IsSuccess = true;
                roomMessageResponse.Message = "message already exists";
                return roomMessageResponse;
            }

            try
            {
                var param = new SendMessaageToRoomRequest();
                param.isRtl = request.isRtl;
                param.attachmentId = request.attachmentId;
                param.message = request.message;

                roomMessageResponse = await _chat.SendMessaageFlightChannel(param);
            }
            catch (Exception ex)
            {
                roomMessageResponse.IsSuccess = false;
                roomMessageResponse.Message = ex.Message;
            }

            return roomMessageResponse;
        }

        public List<Guid> CheckIfUserCanDeleteChatLog(CheckIfUserCanDeleteChatLogRequest request)
        {
            List<Guid> res = new();

            var chatRoom = _chatRoom.Find(x => x.ChatRoomName == request.RoomName).FirstOrDefault();
            if (chatRoom == null)
            {
                //res.IsSuccess = false;
                //res.Message = "Chat room not found";
                return null;
            }

            res = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == chatRoom.Id).Where(x => request.MessageGuids.Contains(x.chatLog.ChatGuid)).Select(x => x.chatLog.ChatGuid).ToList();

            //var foundLogs = _chatLog.Find(x => x.FromUserName == request.UserName && request.MessageGuids.Contains(x.ChatGuid)).Include(x=> x.ChatRoomLogs.Where(y=> y.ChatRoomId==foundChatRoom.Id)).ToList();
            return res;
        }

        public ResponseMessage SetUserRoomPushNotification(SetUserRoomPushNotificationRequest request)
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                var checkMemberResult = _adminService.CheckIfUserIsRoomMember(request.UserName, request.RoomName);

                if (checkMemberResult.IsRoomMember == false)
                {
                    response.IsSuccess = false;
                    response.Message = checkMemberResult.Message;
                    return response;
                }

                var foundUser = _chatRoomMemeber.Find(x => x.ChatRoomId == checkMemberResult.RoomId && x.UserName == request.UserName).FirstOrDefault();

                foundUser.CantGetNotif = !request.CanGetPushNotification;
                _chatRoomMemeber.Update(foundUser);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public GetUserChatRoomNotificationStatusResponse GetUserChatRoomNotificationStatus(GetUserChatRoomNotificationStatusRequest request)
        {
            GetUserChatRoomNotificationStatusResponse response = new();
            try
            {
                var checkMemberResult = _adminService.CheckIfUserIsRoomMember(request.UserName, request.RoomName);

                if (checkMemberResult.IsRoomMember == false)
                {
                    response.IsSuccess = false;
                    response.Message = checkMemberResult.Message;
                    return response;
                }
                var foundUser = _chatRoomMemeber.Find(x => x.ChatRoomId == checkMemberResult.RoomId && x.UserName == request.UserName).FirstOrDefault();


                response.CanGetNotif = !(foundUser.CantGetNotif ?? false);
                response.RoomName = request.RoomName;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }

            return response;

        }

        public async Task<PrivateChatSocketIOResponse> SendMessaageToRoom(SendMessaageToRoomRequest request)
        {

            PrivateChatSocketIOResponse res = new();

            try
            {
                var chatRoom = _chatRoom.Find(x => x.ChatRoomName == request.chatRoomName).FirstOrDefault();

                if (chatRoom == null)
                {
                    res.IsSuccess = false;
                    res.Message = "Room not found";
                    return res;
                }
                else
                {
                    if (chatRoom.IsActive != true)
                    {
                        res.IsSuccess = false;
                        res.Message = "Room is not active any more";
                        return res;
                    }
                }



                var foundMember = _chatRoomMemeber.Find(x => x.UserName == "griffin" && x.ChatRoomId == chatRoom.Id).FirstOrDefault();
                if (foundMember == null)
                {
                    ChatRoomMemeber tmpChatRoomMember = new();
                    tmpChatRoomMember.ChatRoomId = chatRoom.Id;
                    tmpChatRoomMember.UserId = 0;
                    tmpChatRoomMember.UserName = "griffin";
                    tmpChatRoomMember.IsActive = true;
                    tmpChatRoomMember.IsAdmin = true;
                    tmpChatRoomMember.CreateDateTime = DateTime.Now;
                    _chatRoomMemeber.Add(tmpChatRoomMember);

                }


                var tmpChatLog = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => x.chatRoomLog.ChatRoomId == chatRoom.Id).Where(x => x.chatLog.FromUserName == "griffin").OrderByDescending(x => x.chatLog.CreateDate).Select(x => x.chatLog).Take(1).FirstOrDefault();

                if (tmpChatLog != null && tmpChatLog.Message == request.message)
                {
                    if ((DateTime.Now - tmpChatLog.CreateDate).Minutes < 60)
                    {
                        res.Message = "The repetitive message can't be sent within 60 minutes";
                        return res;
                    }
                }
                res = await _robotChat.MessageToChatRoomIOAsRobot(new Request.MessageToChatRoom.MessageToChatRoomIORequest { data = new Request.MessageToChatRoom.RoomData { attachmentId = request.attachmentId, chatRoomName = request.chatRoomName, isRtl = request.isRtl, message = request.message } });

                //res = await _chat.SendMessaageToRoom(request);
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;

            }


            return res;
        }

        public async Task<PrivateChatResponse> PrivateChatBulkMessages(PrivateChatMessageRequest request)
        {
            PrivateChatResponse privateChatResponse = new();


            try
            {
                privateChatResponse = await _chat.PrivateChatBulkMessages(request);
            }
            catch (Exception ex)
            {

                privateChatResponse.IsSuccess = false;
                privateChatResponse.Message = ex.Message;
            }

            return privateChatResponse;
        }

        public AddUsersToRoomResponse AddUsersToRoom(AddUsersToRoomRequest request)
        {
            AddUsersToRoomResponse res = new();

            try
            {
                var foundChatRoom = _chatRoom.Find(x => x.ChatRoomName == request.RoomName).FirstOrDefault();
                if (foundChatRoom == null)
                {
                    res.Message = "Room Not found";
                    res.IsSuccess = false;
                    return res;
                }

                var roomType = _chatRoomType.GetById(foundChatRoom.ChatRoomTypeId);

                if (roomType.IsChannel.Value)
                {
                    var adminMember = _chatRoomMemeber.Find(x => x.ChatRoomId == foundChatRoom.Id && x.UserName == request.requester && x.IsAdmin == true).FirstOrDefault();
                    if (adminMember == null)
                    {
                        res.Message = "requestor is not an admin";
                        res.IsSuccess = false;
                        return res;
                    }
                }

                var memberAddRes = AddMemberToChatRoom(foundChatRoom.Id, request.InviteeUserNames, request.requester);

                res.Warnings = memberAddRes;

                return res;
            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = ex.Message;
                return res;
            }
        }

        async public Task<PoolingV2Response> PoolingV2(PoolingV2Request request)
        {
            var res = new PoolingV2Response();
            var fromDatetime = DateTimeOffset.FromUnixTimeMilliseconds(request.FromTimeStamp);
            var chatStatusDict = _chatStatus.Find(x => true).ToDictionary(x => x.Id, y => y.ChatStatus1);
            List<string?> chatPartners = new List<string?>();
            if (request.FromTimeStamp == 0)
            {
                chatPartners = _chatLog.Find(x => x.ToUserName == request.ToUserName && x.IsDeleted != true).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).GroupBy(x => x.FromUserName).Select(x => x.Select(y => y.FromUserName).First()).ToList();
            }
            else
            {
                chatPartners = _chatLog.Find(x => x.ToUserName == request.ToUserName && x.ClientDateTime > fromDatetime && x.IsDeleted != true).Include(x => x.ChatRoomLogs.Where(y => y.ChatRoomId == 12)).GroupBy(x => x.FromUserName).Select(x => x.Select(y => y.FromUserName).First()).ToList();
            }
            var commaSeperatedUserNames = string.Join(",", chatPartners);
            commaSeperatedUserNames = $"{commaSeperatedUserNames},{request.ToUserName}";
            var resUserDetailsFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = commaSeperatedUserNames });

            Func<GetEployeeDetailsResponse, string, Dictionary<string, object>> GetFullName = (x, i) => x.result.Where(x => x["userName"].ToString() == i).FirstOrDefault();

            var privateChatLogs = _chatLog.Find(x => (chatPartners.Contains(x.FromUserName) && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && chatPartners.Contains(x.ToUserName))).ToList();
            //var resss =  resUserDetailsFrom.result.Where(x => x["userName"].ToString() == "morteza").FirstOrDefault();
            //var mmm =  resUserDetailsFrom.result.Where(x => x["userName"] == "hamed").FirstOrDefault()["fullName"].ToString();
            var chatLogIds = privateChatLogs.Select(x => x.Id).ToList();
            var attachmentLogs = _chatLogAttachment.Find(x => chatLogIds.Contains(x.ChatLogId)).ToList();
            var attachmentIds = attachmentLogs.Select(x => x.ChatAttachmentId).ToList();
            var attachments = _chatAttachment.Find(x => attachmentIds.Contains(x.Id)).ToList();

            Func<List<ChatLogAttachment>, List<ChatAttachment>, long, PoolingV2Attachment?> getChatAttachment = (x, y, z) =>
            {
                PoolingV2Attachment res = null;
                var foundChatAttachmentLog = x.Where(x => x.ChatLogId == z).FirstOrDefault();
                if (foundChatAttachmentLog != null)
                {
                    var foundAttachment = attachments.Where(x => x.Id == foundChatAttachmentLog.ChatAttachmentId).FirstOrDefault();
                    if (foundAttachment != null)
                    {
                        res = new()
                        {
                            id = foundAttachment.Id,
                            contentType = foundAttachment.ContentType,
                            name = foundAttachment.FileName,
                            size = foundAttachment.FileSize,
                            type = foundAttachment.FileType
                        };
                    }
                }


                return res;

            };


            foreach (var i in chatPartners)
            {

                //i["pkEmployee"].ToString(), i["fullName"].ToString()
                PoolingV2ResponseDetails tmpDetails = new();
                var countOfUnread = privateChatLogs.Where(x => x.FromUserName == i && x.ChatStatusId == 1).Count();
                var foundFirstLog = privateChatLogs.Where(x => x.FromUserName == i).FirstOrDefault();
                //var foundUserDetail = resUserDetailsFrom.result.Where(x => x["userName"].ToString() == i).FirstOrDefault();
                var foundUserDetail = GetFullName(resUserDetailsFrom, i);


                var fullname = "";
                if (foundUserDetail == null)
                {
                    continue;
                }
                else
                {
                    fullname = foundUserDetail["fullName"].ToString();
                }

                var tmpRoom = tmpDetails.room = new PoolingV2Room()
                {
                    type = "private",
                    username = i,
                    remoteId = foundFirstLog.FromEmpId,
                    role = "Admin",
                    unreadCount = countOfUnread,
                    fullName = fullname
                };


                var messages = tmpDetails.message = new PoolingV2Message();


                messages.total = 0;
                messages.list = new List<PoolingV2MessageDetail>();
                Func<ChatLog, bool> chatLogCondition = x => (x.FromUserName == i && x.ToUserName == request.ToUserName) || (x.FromUserName == request.ToUserName && x.ToUserName == i);


                Func<ChatLog, PoolingV2MessageDetail> pv2Message = x => new PoolingV2MessageDetail()
                {
                    date = x.ClientDateTime.Value.ToUnixTimeMilliseconds(),
                    forwardedBy = x.ForwardedBy,
                    fromFullName = GetFullName(resUserDetailsFrom, x.FromUserName)["fullName"].ToString(),
                    guid = x.ChatGuid,
                    fromUserName = x.FromUserName,
                    isRtl = x.IsRtl,
                    replyOf = x.Reply,
                    fromUserRemoteId = x.FromEmpId,
                    status = chatStatusDict[x.ChatStatusId],
                    attachment = getChatAttachment(attachmentLogs, attachments, x.Id),
                    message = x.Message

                };


                messages.total = privateChatLogs.Where(x => chatLogCondition(x)).Count();
                if (messages.total > 30)
                {
                    messages.list = privateChatLogs.Where(x => chatLogCondition(x)).OrderByDescending(x => x.ClientDateTime).Take(30).Select(x => pv2Message(x)).ToList();
                }
                else
                {
                    messages.list = privateChatLogs.Where(x => chatLogCondition(x)).OrderByDescending(x => x.ClientDateTime).Select(x => pv2Message(x)).ToList();
                }


                res.result.Add(tmpDetails);

            }





            var roomList = _chatRoomMemeber.Find(x => x.UserName == request.ToUserName).Include(x => x.ChatRoom).Where(x => x.ChatRoom.IsActive == true).Select(x => x.ChatRoom).ToList();

            var roomTypeIds = roomList.Select(x => x.ChatRoomTypeId).ToList();
            var roomTypeDict = _chatRoomType.Find(x => roomTypeIds.Contains(x.Id)).ToDictionary(y => y.Id, z => z.Name);

            var membershipDetails = _chatRoomMemeber.Find(x => x.UserName == request.ToUserName && roomTypeIds.Contains(x.ChatRoomId)).ToList();

            List<Dictionary<string, object>> tmpUserChatRoomList = new();
            var roomListIds = roomList.Select(x => x.Id).ToList();
            //var tmpChatLogList = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => roomListIds.Contains(x.chatRoomLog.ChatRoomId)).Where(x => x.chatLog.ClientDateTime > fromDatetime).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new { x.chatLog, chatRoomId = x.chatRoomLog.ChatRoomId }).ToList();
            List<ChatLogWithRoomId> tmpChatLogList = _context.ChatLogs.Join(_context.ChatRoomLogs, chatLog => chatLog.Id, chatRoomLog => chatRoomLog.ChatLogId, (chatLog, chatRoomLog) => new { chatLog, chatRoomLog }).Where(x => roomListIds.Contains(x.chatRoomLog.ChatRoomId)).Where(x => x.chatLog.ClientDateTime > fromDatetime).OrderByDescending(x => x.chatLog.CreateDate).Select(x => new ChatLogWithRoomId() { chatLog = x.chatLog, ChatRoomId = x.chatRoomLog.ChatRoomId }).ToList();
            var chatRoomFromUsernames = tmpChatLogList.Select(x => x.chatLog.FromUserName).Distinct().ToList();
            var commaSeperatedRoomSenders = string.Join(",", chatRoomFromUsernames);

            var resUserDetailsRoomFrom = await _griffinAirAvation.GetEployeeDetails(new GetEployeeDetailsRequest { UserName = commaSeperatedRoomSenders });

            var chatLogIdsForChatRoom = tmpChatLogList.Select(x => x.chatLog.Id).ToList();

            var tmpChatLogListIds = tmpChatLogList.Select(x => x.chatLog.Id).ToList();
            var foundChatLogAttachments = _chatLogAttachment.Find(x => tmpChatLogListIds.Contains(x.ChatLogId)).ToList();

            var foundAttachmentIds = foundChatLogAttachments.Select(x => x.ChatAttachmentId).ToList();
            var foundAttachmens = _chatAttachment.Find(x => foundAttachmentIds.Contains(x.Id)).ToList();

            var maxChatLog = tmpChatLogList.Max(x => x.chatLog.Id);

            var userChatRoomRecievers = _userChatRoomReciever.Find(x => roomListIds.Contains(x.ChatroomId) && x.UserName == request.ToUserName && x.ChatLogId <= maxChatLog).ToList();


            var userChatRoomRecieversUnread = userChatRoomRecievers.Where(x => roomListIds.Contains(x.ChatroomId) && x.UserName == request.ToUserName && x.ChatStatusId == 1 && x.ChatLogId <= maxChatLog).ToList();

            var attachmentLogsForChatRoom = _chatLogAttachment.Find(x => chatLogIdsForChatRoom.Contains(x.ChatLogId)).ToList();
            var chatAttachmentIds = attachmentLogsForChatRoom.Select(x => x.ChatAttachmentId).ToList();

            var chatAttachments = _chatAttachment.Find(x => chatAttachmentIds.Contains(x.Id)).ToList();

            Func<List<ChatLogWithRoomId>, long, List<PoolingV2MessageDetail>> chatRoomMessages = (x, y) =>
            {


                var res = x.Where(x => x.ChatRoomId == y).Select(x => new PoolingV2MessageDetail()
                {
                    attachment = getChatAttachment(attachmentLogsForChatRoom, chatAttachments, x.chatLog.Id),
                    forwardedBy = x.chatLog.ForwardedBy,
                    date = x.chatLog.ClientDateTime.Value.ToUnixTimeMilliseconds(),
                    fromFullName = GetFullName(resUserDetailsRoomFrom, x.chatLog.FromUserName) == null ? "" : GetFullName(resUserDetailsRoomFrom, x.chatLog.FromUserName)["fullName"].ToString(),
                    fromUserName = x.chatLog.FromUserName,
                    guid = x.chatLog.ChatGuid,
                    isRtl = x.chatLog.IsRtl,
                    replyOf = x.chatLog.Reply,
                    fromUserRemoteId = -1,
                    message = x.chatLog.Message,
                    status = "seen"
                }).ToList();
                return res;
            };


            foreach (var i in roomList)
            {
                PoolingV2ResponseDetails tmpDetails = new();
                string theRole = "member";
                if (i.CreatorUserName == request.ToUserName)
                {
                    theRole = "owner";
                }
                else
                {
                    var foundMem = membershipDetails.Where(x => x.ChatRoomId == i.Id && x.UserName == request.ToUserName).FirstOrDefault();
                    if (foundMem != null)
                    {
                        if (foundMem.IsAdmin == true)
                        {
                            theRole = "admin";
                        }
                    }
                }
                var countOfUnreadMessages = userChatRoomRecieversUnread.Where(x => x.ChatroomId == i.Id).Count();

                var tmpRoom = tmpDetails.room = new PoolingV2Room()
                {
                    fullName = i.ChatRoomName,
                    username = i.ChatRoomName,
                    remoteId = null,
                    type = roomTypeDict[i.ChatRoomTypeId.Value].ToString().ToLower(),
                    role = theRole,
                    unreadCount = countOfUnreadMessages
                };



                tmpDetails.message = new PoolingV2Message()
                {
                    total = 0,
                    list = chatRoomMessages(tmpChatLogList, i.Id)
                };


                res.result.Add(tmpDetails);

            }

            return res;

        }

        public void UploadToChatMinio()
        {

        }





    }

}
