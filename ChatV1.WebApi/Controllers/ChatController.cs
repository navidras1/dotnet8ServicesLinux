using Asp.Versioning;
using Azure.Core;
using Azure.Identity;
using ChatV1.Service.Model.ChatRoom;
using ChatV1.Service.Request;
using ChatV1.Service.Request.MessageToChatRoom;
using ChatV1.Service.Request.PrivateChatSocketIO;
using ChatV1.Service.Response;
using ChatV1.Service.Services;
using ChatV1.Service.Services.Minio;
using ChatV1.Service.Services.SocketIo;
using ChatV1.WebApi.Models;
using CommunityToolkit.HighPerformance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog.LayoutRenderers.Wrappers;
using System.IO;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatV1.WebApi.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private IActions _actions;
        private readonly ILogger<ChatController> _logger;
        private IHostEnvironment _hostingEnvironment;
        private IChat _chat;
        private readonly IAdminService _adminService;
        private IMinIOService _minIOService;


        public ChatController(IActions actions, ILogger<ChatController> logger, IHostEnvironment hostingEnvironment, IChat chat, IAdminService adminService, [FromKeyedServices("ChatMinIO")] IMinIOService minIOService)
        {
            _actions = actions;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _chat = chat;
            _adminService = adminService;
            _minIOService = minIOService;
        }

        [HttpPost("UserChatHistory")]
        public IActionResult UserChatHistory()
        {
            //var name =  User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var mm = User.Identity.Name;
            GetChatHistoryRequest request = new GetChatHistoryRequest();
            request.UserId = Convert.ToInt32(userId);


            var res = _actions.GetChatHistory(request);
            return Ok(res);
        }

        [HttpPost("GetUserUreadMessages")]
        public IActionResult GetUserUreadMessages()
        {
            var userName = User.Identity.Name;
            GetUnreadMessagesRequest request = new();
            request.UserName = userName;
            var res = _actions.GetUnreadMessages(request);
            return Ok(res);
        }

        [HttpPost("UpdateUserToReadMessag")]
        public IActionResult UpdateUserToReadMessag(UpdateUserToReadMessagRequest request)
        {
            var userName = User.Identity.Name;

            request.UserName = userName.ToLower();
            var res = _actions.UpdateUserToReadMessag(request);
            return Ok(res);

        }

        [HttpPost("GetLastMessage")]
        public IActionResult GetLastMessage(GetLastMessageRequest request)
        {
            var userName = User.Identity.Name;
            request.toUser = userName;
            var res = _actions.GetLastMessage(request);
            return Ok(res);

        }

        [HttpPost("HistoryMessageOfUsers")]
        public async Task<IActionResult> HistoryMessageOfUsers(HistoryMessageOfUsersRequest request)
        {
            var userName = User.Identity.Name;
            request.toUser = userName.ToLower();
            request.requestor = userName.ToLower();

            _logger.LogInformation("testtttttttttt " + JsonConvert.SerializeObject(request));

            var res = await _actions.HistoryMessageOfUsers(request);
            return Ok(res);

        }

        [HttpPost("HistoryMessageOfUsers"), ApiVersion("2.0")]
        public async Task<IActionResult> HistoryMessageOfUsersV2(HistoryMessageOfUsersRequestV2 request)
        {
            var userName = User.Identity.Name;
            request.toUser = userName.ToLower();
            request.requestor = userName.ToLower();

            _logger.LogInformation("testtttttttttt " + JsonConvert.SerializeObject(request));

            var res = await _actions.HistoryMessageOfUsersV2(request);
            return Ok(res);

        }

        [HttpPost("CreateChannelGroupRoom")]
         async public Task<IActionResult> CreateChannelGroupRoom([FromForm] CreateRoomRequest request)
        {
            var sizeLimit = 60 * 1024 * 1024;
            if (request.TheFile.Length > sizeLimit)
            {
                PrivateChatUploadFileResponse resError = new();
                resError.IsSuccess = false;

                resError.Message = $"File size limit {sizeLimit}";
                return StatusCode(((int)HttpStatusCode.NotAcceptable), resError);

            }

            var theName = request.TheFile.FileName;
            //var fromUserName = User.Identity.Name.ToLower();
            var serviceReq = new FileUploadPrivateChatRequest();
            //serviceReq.FromUserName = fromUserName;
            //serviceReq.ToUserName = request.ToUserName.ToLower();
            //serviceReq.Message = request.Message;
            serviceReq.FileName = request.TheFile.FileName.ToLower();
            serviceReq.FileExtension = Path.GetExtension(serviceReq.FileName).ToLower();


            var lastIndexOfDot = request.TheFile.FileName.LastIndexOf(".");
            var theExtension = request.TheFile.FileName.Substring(lastIndexOfDot);
            var theNameWithoutExtension = request.TheFile.FileName.Substring(0, lastIndexOfDot);

            DateTime currentTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
            //var newNameExtension =  $"{theNameWithoutExtension}_{unixTime}{theExtension}";

            var uploadFileName = $"{theNameWithoutExtension}_{unixTime}{theExtension}";

            var fileStream = request.TheFile.OpenReadStream();

            var uploadFileres = await _minIOService.UploadFileAsync(fileStream, uploadFileName, theExtension, request.TheFile.ContentType);

            var attachRes = _actions.PrivateChatUploadFile(new PrivateChatUploadFileRequest
            {
                ContentType = request.TheFile.ContentType,
                FileName = uploadFileName,
                FileExtension = theExtension,
                FilePath = uploadFileres.FileAddress,
                FileSize = request.TheFile.Length,
                MinioBucket = uploadFileres.BucketName
            });

            var userName = User.Identity.Name.ToLower();

            request.CreatorUserName = userName;
            request.LogoId = attachRes.AttachmentId;

            var res = _actions.CreateChannelGroupRoom(request);
            res.PicId = attachRes.AttachmentId;
            return Ok(res);

        }

        [HttpPost("DisableRoom")]
        public IActionResult DisableRoom(DisableRoomRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.OwnerName = userName;

            var res = _actions.DisableRoom(request);
            return Ok(res);
        }




        [HttpPost("GetUserRooms")]
        public IActionResult GetUserRooms()
        {
            var userName = User.Identity.Name.ToLower();
            GetUserRoomsRequest request = new GetUserRoomsRequest();
            request.UserName = userName;
            var res = _actions.GetUserRooms(request);
            return Ok(res);
        }

        [HttpPost("GetUserRooms"), ApiVersion("2.0")]
        public IActionResult GetUserRoomsV2()
        {
            var userName = User.Identity.Name.ToLower();
            GetUserRoomsRequest request = new GetUserRoomsRequest();
            request.UserName = userName;
            var res = _actions.GetUserRoomsV2(request);
            return Ok(res);
        }

        [HttpPost("GetListOfRoomsWithUnreadMessages")]
        public IActionResult GetListOfRoomsWithUnreadMessages()
        {


            var userName = User.Identity.Name.ToLower();

            var res = _actions.GetUsersChatRoomsWithCountOfUnreads(userName.ToLower());
            return Ok(res);
        }

        [HttpPost("GetListOfRoomsWithUnreadMessages"), ApiVersion(2)]
        [ProducesResponseType<UsersChatRoomsWithCountOfUnreadsResponseV2>(StatusCodes.Status200OK)]
        public IActionResult GetListOfRoomsWithUnreadMessagesV2(ListOfRoomsWithUnreadMessagesRequestV2 request)
        {
            var userName = User.Identity.Name.ToLower();
            request.userName = userName;

            var res = _actions.GetUsersChatRoomsWithCountOfUnreadsV2(request);
            return Ok(res);
        }

        [HttpPost("GetListOfRoomsWithUnreadMessages"), ApiVersion(4)]
        public IActionResult GetListOfRoomsWithUnreadMessagesV4(GetListOfRoomsWithUnreadMessagesRequestV3 request)
        {

            var res = _actions.GetUsersChatRoomsWithCountOfUnreads(request.UserName);
            return Ok(res);
        }


        [HttpPost("GetListOfRoomsWithUnreadMessages"), ApiVersion(3)]
        public ActionResult GetUsersChatRoomsWithCountOfUnreadsV3(ListOfRoomsWithUnreadMessagesRequestV3ForUser request)
        {
            var userName = User.Identity.Name.ToLower();
            ListOfRoomsWithUnreadMessagesRequestV3 req = new() { fromTimeStamp = request.fromTimeStamp, userName = userName };
            var res = _actions.GetUsersChatRoomsWithCountOfUnreadsV3(req);
            return Ok(res);
        }

        [HttpPost("GetUsersChatroomMessages")]
        public IActionResult GetUsersChatroomMessages(GetUsersChatroomMessagesRequest request)
        {
            request.UserName = User.Identity.Name.ToLower();

            var res = _actions.GetUsersChatroomMessages(request);
            return Ok(res);
        }

        [HttpPost("GetUsersChatroomMessages"), ApiVersion(2)]
        public IActionResult GetUsersChatroomMessagesV2(GetUsersChatroomMessagesRequest request)
        {
            request.UserName = User.Identity.Name.ToLower();

            var res = _actions.GetUsersChatroomMessages(request);
            return Ok(res);
        }

        [HttpPost("AddToContactList")]
        public async Task<IActionResult> AddToContactList(AddToContactListRequestForController req)
        {
            AddToContactListRequest request = new AddToContactListRequest();
            request.OwnerUserName = User.Identity.Name.ToLower();
            request.Usernames = req.Usernames;
            var res = await _actions.AddToContactList(request);
            return Ok(res);
        }

        [HttpPost("RemoveFromContacts")]
        public IActionResult RemoveFromContacts(RemoveFromContactsRequestForController req)
        {

            RemoveFromContactsRequest request = new RemoveFromContactsRequest();
            request.OwnerUserName = User.Identity.Name.ToLower();
            request.Usernames = req.Usernames;
            var res = _actions.RemoveFromContacts(request);
            return Ok(res);
        }

        [HttpPost("GetContactList")]
        public async Task<IActionResult> GetContactList(GetContactListRequestForUser req)
        {
            var request = new GetContactListRequest();
            request.OwnerUserName = User.Identity.Name.ToLower();
            request.PageNumber = req.PageNumber;
            request.PageSize = req.PageSize;

            var res = await _actions.GetContactList(request);
            return Ok(res);
        }

        [HttpPost("GetCountAndLastMessagePrivateMessage")]
        public IActionResult GetCountAndLastMessagePrivateMessage(GetCountAndLastMessagePrivateMessageRequestForUser req)
        {
            var request = new GetCountAndLastMessagePrivateMessageRequest();
            request.ToUserName = User.Identity.Name.ToLower();

            request.PageNumber = req.PageNumber;
            request.PageSize = req.PageSize;

            var res = _actions.GetCountAndLastMessagePrivateMessage(request);
            return Ok(res);
        }

        [HttpPost("GetCountAndLastMessagePrivateMessageV2")]
        public IActionResult GetCountAndLastMessagePrivateMessageV2(GetCountAndLastMessagePrivateMessageRequestForUser req)
        {
            var request = new GetCountAndLastMessagePrivateMessageRequest();
            request.ToUserName = User.Identity.Name.ToLower();

            request.PageNumber = req.PageNumber;
            request.PageSize = req.PageSize;

            var res = _actions.GetCountAndLastMessagePrivateMessageV2(request);
            return Ok(res);
        }

        [HttpPost("GetUserChatHistoryV2")]
        public async Task<IActionResult> GetUserChatHistoryV2(GetCountAndLastMessagePrivateMessageRequestForUser req)
        {
            var request = new GetCountAndLastMessagePrivateMessageRequest();
            request.ToUserName = User.Identity.Name.ToLower();

            request.PageNumber = req.PageNumber;
            request.PageSize = req.PageSize;

            var res = await _actions.GetUserChatHistoryV2(request);
            return Ok(res);
        }

        [HttpPost("GetUserChatHistoryV3")]
        public async Task<IActionResult> GetUserChatHistoryV3(GetCountAndLastMessagePrivateMessageRequestForUser req)
        {
            var request = new GetCountAndLastMessagePrivateMessageRequest();
            request.ToUserName = User.Identity.Name.ToLower();

            request.PageNumber = req.PageNumber;
            request.PageSize = req.PageSize;

            var res = await _actions.GetUserChatHistoryV3(request);
            return Ok(res);
        }

        [HttpPost("GetUserChatHistoryV3"), ApiVersion("2")]
        public async Task<IActionResult> GetUserChatHistoryV3_V2(GetCountAndLastMessagePrivateMessageRequestForUserV2 req)
        {
            var request = new GetCountAndLastMessagePrivateMessageRequestV2();
            request.ToUserName = User.Identity.Name.ToLower();
            request.FromTimeStamp = req.FromTimeStamp;
            //request.PageNumber = req.PageNumber;
            //request.PageSize = req.PageSize;

            var res = await _actions.GetUserChatHistoryV3_V2(request);
            return Ok(res);
        }

        [HttpPost("GetUserChatHistoryUnreadMessage")]
        public async Task<IActionResult> GetUserChatHistoryUnreadMessage()
        {
            GetUserChatHistoyUreadMessageRequest request = new GetUserChatHistoyUreadMessageRequest();
            request.ToUserName = User.Identity.Name.ToLower();
            var res = await _actions.GetUserChatHistoryUnreadMessage(request);
            return Ok(res);
        }

        [HttpPost("GetUserChatHistoryUnreadMessageV2")]
        public async Task<IActionResult> GetUserChatHistoryUnreadMessageV2()
        {
            GetUserChatHistoyUreadMessageRequest request = new GetUserChatHistoyUreadMessageRequest();
            request.ToUserName = User.Identity.Name.ToLower();
            var res = await _actions.GetUserChatHistoryUnreadMessageV2(request);
            return Ok(res);
        }

        [HttpPost("GetAllEmployeeForChat")]
        public async Task<IActionResult> GetAllEmployeeForChat(GetAllEmployeeForChatRequestForUser req)
        {
            var request = new GetUsersToChatRequest();
            request.toUserName = User.Identity.Name.ToLower();

            request.userName = req.userName;
            request.pageSize = req.pageSize;
            request.pageNumber = req.pageNumber;
            var res = await _actions.GetAllEmployeeForChat(request);
            return Ok(res);
        }

        [HttpPost("SetLastEmpLastSeen")]
        public IActionResult SetLastEmpLastSeen(SetLastEmpLastSeenRequestForUser req)
        {
            SetLastEmpLastSeenRequest request = new SetLastEmpLastSeenRequest();
            request.UserName = User.Identity.Name.ToLower();
            request.LastSeenDate = req.LastSeenDate;
            var res = _actions.SetLastEmpLastSeen(request);

            return Ok(res);
        }

        [HttpPost("GetLastSeenOfEmp")]
        public IActionResult GetLastSeenOfEmp(GetLastSeenOfEmpRequest request)
        {
            var res = _actions.GetLastSeenOfEmp(request);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("UploadFilePrivateChat")]
        //[RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> UploadFilePrivateChat([FromForm] FileUploadPrivateChatRequestForController request)
        {
            var sizeLimit = 60 * 1024 * 1024;
            if (request.TheFile.Length > sizeLimit)
            {
                PrivateChatUploadFileResponse resError = new();
                resError.IsSuccess = false;

                resError.Message = $"File size limit {sizeLimit}";
                return StatusCode(((int)HttpStatusCode.NotAcceptable), resError);

            }

            var theName = request.TheFile.FileName;
            //var fromUserName = User.Identity.Name.ToLower();
            var serviceReq = new FileUploadPrivateChatRequest();
            //serviceReq.FromUserName = fromUserName;
            //serviceReq.ToUserName = request.ToUserName.ToLower();
            //serviceReq.Message = request.Message;
            serviceReq.FileName = request.TheFile.FileName.ToLower();
            serviceReq.FileExtension = Path.GetExtension(serviceReq.FileName).ToLower();


            var lastIndexOfDot = request.TheFile.FileName.LastIndexOf(".");
            var theExtension = request.TheFile.FileName.Substring(lastIndexOfDot);
            var theNameWithoutExtension = request.TheFile.FileName.Substring(0, lastIndexOfDot);

            DateTime currentTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
            //var newNameExtension =  $"{theNameWithoutExtension}_{unixTime}{theExtension}";

            var uploadFileName = $"{theNameWithoutExtension}_{unixTime}{theExtension}";
            //var name = fromFile.FileName;
            var root = _hostingEnvironment.ContentRootPath;
            if (!Directory.Exists($"{root}/uploads"))
            {
                Directory.CreateDirectory($"{root}/uploads");
            }

            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
            string filePath = Path.Combine(uploads, uploadFileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.TheFile.CopyToAsync(fileStream);
            }
            var req = new PrivateChatUploadFileRequest();
            req.FilePath = filePath;
            req.FileName = uploadFileName;
            req.FileExtension = Path.GetExtension(req.FileName).ToLower();
            req.FileSize = request.TheFile.Length;
            req.ContentType = request.TheFile.ContentType;
            var res = _actions.PrivateChatUploadFile(req);


            return Ok(res);
        }



        [HttpPost("DownloadAttachment")]
        public ActionResult DownloadAttachment(DownloadAttachmentRequestForUser request)
        {
            var toUserName = User.Identity.Name.ToLower();

            var res = _actions.GetAttachmentDetails(new DownloadAttachmentRequest { AttachmentId = request.AttachmentId, ToUserName = toUserName });



            return Ok(res);
        }

        //[AllowAnonymous]
        [HttpPost("DownloadAttachmentTest")]
        public async Task<ActionResult> DownloadAttachmentTest(DownloadAttachmentRequestForUser request)
        {
            //var toUserName = User.Identity.Name.ToLower();

            var res = await _actions.GetAttachmentDetailsTest(new DownloadAttachmentRequestTest { AttachmentId = request.AttachmentId });

            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("GetAttachmentDetailsTestFile")]
        public async Task<ActionResult> GetAttachmentDetailsTestFile(DownloadAttachmentRequestTest request)
        {
            var res = await _actions.GetAttachmentDetailsTestFile(request);
            return File(res.FileStream, "application/octet-stream", res.FileName); // returns a FileStreamResult

        }


        [HttpPost("GetAllEmployeesForChat")]
        public async Task<ActionResult> GetAllEmployeesForChat(GetAllEmployeesForChatForUser request)
        {
            GetAllEmployeeForChatRequest req = new GetAllEmployeeForChatRequest();

            req.MasterUserName = User.Identity.Name.ToLower();
            req.UserName = request.UserName;
            req.PageNumber = request.PageNumber;
            req.PageSize = request.PageSize;
            req.ToUserName = request.ToUserName;
            var res = await _actions.GetAllEmployeeForChat(req);
            return Ok(res);
        }

        [HttpPost("GetAllEmployeesForChat"), ApiVersion(2)]
        public async Task<ActionResult> GetAllEmployeesForChatV2(GetAllEmployeesForChatForUser request)
        {
            GetAllEmployeeForChatRequest req = new GetAllEmployeeForChatRequest();

            req.MasterUserName = User.Identity.Name.ToLower();
            req.UserName = request.UserName;
            req.PageNumber = request.PageNumber;
            req.PageSize = request.PageSize;
            req.ToUserName = request.ToUserName;
            var res = await _actions.GetAllEmployeeForChatV2(req);
            return Ok(res);
        }


        [HttpPost("HistoryChatOfUsers")]
        public async Task<ActionResult> HistoryChatOfUsers(Dictionary<string, Guid?> req)
        {
            var userName = User.Identity.Name.ToLower();
            var res = await _actions.HistoryChatOfUsers(userName, req);
            return Ok(res);
        }

        [HttpPost("HistoryChatOfUsers"), ApiVersion("2")]
        public async Task<ActionResult> HistoryChatOfUsersV2(List<HistoryChatRequestDetail> req)
        {
            var userName = User.Identity.Name.ToLower();
            var res = await _actions.HistoryChatOfUsersV2(new HistoryChatOfUsersV2Request { RquestDetails = req, UserName = userName });
            return Ok(res);
        }



        [HttpPost("SendPrivateChatMessage")]
        public async Task<ActionResult> SendPrivateChatMessage(PrivateChatMessageRequest request)
        {
            //var userName = User.Identity.Name.ToLower();
            //if (_adminService.IsSuperUser(userName) == true)
            //{

            var res = await _actions.PrivateChatService(request);
            return Ok(res);
            //}
            //else
            //{
            //    return Unauthorized("User is not a SuperUser");
            //}

        }

        [HttpPost("PrivateChatToUsers")]
        public async Task<ActionResult> PrivateChatToUsers(PrivateChatMessageToUsersRequest request)
        {
            var res = await _actions.PrivateChatToUsersService(request);
            return Ok(res);
        }

        [HttpPost("GetFileDetail")]
        public ActionResult GetFileDetail(GetFileDetailRequest request)
        {
            var res = _actions.GetFileDetail(request);
            return Ok(res);
        }

        [HttpPost("UpdateChatRoomMessagesToRead")]
        public ActionResult UpdateChatRoomMessagesToRead(UpdateChatRoomMessagesToReadRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.UserName = userName;
            var res = _actions.UpdateChatRoomMessagesToRead(request);
            return Ok(res);

        }

        [HttpPost("UserRoomHistory")]
        public async Task<ActionResult> UserRoomHistoryAsync(UserRoomHistoryRequestForUser request)
        {
            var userName = User.Identity.Name.ToLower();
            UserRoomHistoryRequest req = new()
            {
                UserName = userName,
                Chatguid = request.Chatguid,
                ChatRoomName = request.ChatRoomName
            };

            var res = await _actions.UserRoomHistory(req);
            return Ok(res);
        }

        [HttpPost("UserRoomHistory"), ApiVersion("2.0")]
        public async Task<ActionResult> UserRoomHistoryAsyncV2(UserRoomHistoryRequestV3 request)
        {
            var userName = User.Identity.Name.ToLower();
            //UserRoomHistoryRequest req = new()
            //{
            //    UserName = userName,
            //    Chatguid = request.Chatguid,
            //    ChatRoomName = request.ChatRoomName
            //};
            request.UserName = userName;

            var res = await _actions.UserRoomHistoryV2(request);
            return Ok(res);
        }

        [HttpPost("SendMessaageToRoom")]
        public async Task<ActionResult> SendMessaageToRoom(SendMessaageToRoomRequest request)
        {
            //var res = await _chat.SendMessaageToRoom(request);
            var res = await _actions.SendMessaageToRoom(request);

            return Ok(res);
        }



        [HttpPost("RoomMembers")]
        public async Task<ActionResult> RoomMembers(RoomDetail request)
        {
            var userName = User.Identity.Name.ToLower();
            var res = await _actions.RoomMembers(new RoomMembersRequest { UserName = userName, RoomDetail = request });
            return Ok(res);
        }

        [HttpPost("DeletePrivateChat")]
        public ActionResult DeletePrivateChat(DeletePrivateChatServiceRequest request)
        {
            var res = _actions.DeletePrivateChat(request);
            return Ok(res);
        }

        [HttpPost("SendMessageToFlighChannel")]
        public async Task<IActionResult> SendMessageToFlighChannel(SendMessageToFlighChannelRequest request)
        {
            var res = await _actions.SendMessageToFlighChannel(request);
            return Ok(res);
        }



        [HttpPost("DeleteUsersFromRoom")]
        public ActionResult DeleteUsersFromRoom(DeleteUserFromRoomRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.Requestor = userName;
            var res = _actions.DeleteUserFromRoom(request);

            return Ok(res);

        }

        [HttpPost("MakeUserAdminOrNot")]
        public ActionResult MakeUsersAdminOrNot(MakeUsersAdminOrNotRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.requester = userName;
            var res = _actions.MakeUsersAdminOrNot(request);
            return Ok(res);
        }


        [HttpPost("CheckOfflineActions")]
        public ActionResult CheckOfflineActions()
        {
            var userName = User.Identity.Name.ToLower();
            var res = _actions.CheckOfflineActions(userName);
            return Ok(res);
        }


        [HttpPost("InstertToOffLineAction")]
        public ActionResult InstertToOffLineAction(InstertToOffLineActionRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.FromUserName = userName;
            var res = _actions.InstertToOffLineAction(request);
            return Ok(res);
        }

        [HttpPost("UpdateOfflineActionByType")]
        public ActionResult UpdateOfflineActionByType(UpdateOfflineActionByTypeRequest request)
        {
            var userName = User.Identity.Name.ToLower();

            return Ok();
        }

        [HttpPost("UpdateMessageToReadFromTo")]
        public ActionResult UpdateMessageToReadFromTo(UpdateMessageToReadFromToRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.FromUserName = userName;
            var res = _actions.UpdateMessageToReadFromTo(request);
            return Ok(res);
        }

        [HttpPost("DeleteChatOffline")]
        public ActionResult DeleteChatOffline(DeleteChatOfflineRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.FromUsername = userName;
            var res = _actions.DeleteChatOffline(request);
            return Ok(res);
        }

        [HttpPost("MakeOfflineActionDone")]
        public ActionResult MakeOfflineActionDone()
        {
            var userName = User.Identity.Name.ToLower();
            MakeOfflineActionDoneRequest request = new() { UserName = userName };
            var res = _actions.MakeOfflineActionDone(request);
            return Ok(res);

        }

        [HttpPost("Pooling")]
        public async Task<ActionResult> Pooling(GetCountAndLastMessagePrivateMessageRequestV2 request)
        {
            request.ToUserName = User.Identity.Name.ToLower();
            var res = await _actions.Pooling(request);
            return Ok(res);
        }

        [HttpPost("Pooling"), ApiVersion("2.0")]
        public  async Task<ActionResult> PoolingV2(PoolingV2Request request)
        {
            request.ToUserName = User.Identity.Name.ToLower();
            var res = await _actions.PoolingV2(request);
            return Ok(res);
        }

        [HttpPost("RoomMembersWithNotif")]
        public async Task<ActionResult> RoomMembersWithNotif(RoomDetail request)
        {

            var userName = User.Identity.Name.ToLower();
            var res = await _actions.RoomMembersWithNotif(new RoomMembersRequest { UserName = userName, RoomDetail = request });
            return Ok(res);
        }

        [HttpPost("RoomMembersWithNotif") , ApiVersion("2.0")]
        public async Task<ActionResult> RoomMembersWithNotifV2(RoomDetailV2 request)
        {

            var userName = User.Identity.Name.ToLower();
            var res = await _actions.RoomMembersWithNotifV2(new RoomMembersRequestV2 { UserName = userName, RoomDetail = request });
            return Ok(res);
        }

        [HttpPost("SetUserRoomPushNotification")]
        public ActionResult SetUserRoomPushNotification(SetUserRoomPushNotificationRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.UserName = userName;
            var res = _actions.SetUserRoomPushNotification(request);
            return Ok(res);
        }

        [HttpPost("GetUserChatRoomNotificationStatus")]
        public ActionResult GetUserChatRoomNotificationStatus(GetUserChatRoomNotificationStatusRequest request)
        {

            var userName = User.Identity.Name.ToLower();
            request.UserName = userName;

            var res = _actions.GetUserChatRoomNotificationStatus(request);
            return Ok(res);
        }

        [HttpPost("PrivateChatBulkMessages")]
        public async Task<ActionResult> PrivateChatBulkMessages(PrivateChatMessageRequest request)
        {
            var res = await _actions.PrivateChatBulkMessages(request);
            return Ok(res);
        }

        [HttpPost("PrivateChatSocketIO")]
        public async Task<ActionResult> PrivateChatSocketIO(PrivateChatSocketIORequest request)
        {
            var res = await _chat.PrivateChatSocketIO(request);
            return Ok(res);
        }

        [HttpPost("PrivateChatSocketIOWithFile")]
        public async Task<ActionResult> PrivateChatSocketIOWithFile([FromForm] PrivateChatSocketIOWithFileRequest request)
        {
            var sizeLimit = 60 * 1024 * 1024;
            if (request.TheFile.Length > sizeLimit)
            {
                PrivateChatUploadFileResponse resError = new();
                resError.IsSuccess = false;

                resError.Message = $"File size limit {sizeLimit}";
                return StatusCode(((int)HttpStatusCode.NotAcceptable), resError);

            }

            var theName = request.TheFile.FileName;
            //var fromUserName = User.Identity.Name.ToLower();
            var serviceReq = new FileUploadPrivateChatRequest();
            //serviceReq.FromUserName = fromUserName;
            //serviceReq.ToUserName = request.ToUserName.ToLower();
            //serviceReq.Message = request.Message;
            serviceReq.FileName = request.TheFile.FileName.ToLower();
            serviceReq.FileExtension = Path.GetExtension(serviceReq.FileName).ToLower();


            var lastIndexOfDot = request.TheFile.FileName.LastIndexOf(".");
            var theExtension = request.TheFile.FileName.Substring(lastIndexOfDot);
            var theNameWithoutExtension = request.TheFile.FileName.Substring(0, lastIndexOfDot);

            DateTime currentTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
            //var newNameExtension =  $"{theNameWithoutExtension}_{unixTime}{theExtension}";

            var uploadFileName = $"{theNameWithoutExtension}_{unixTime}{theExtension}";

            var fileStream = request.TheFile.OpenReadStream();

            var uploadFileres = await _minIOService.UploadFileAsync(fileStream, uploadFileName, theExtension, request.TheFile.ContentType);

            var attachRes = _actions.PrivateChatUploadFile(new PrivateChatUploadFileRequest
            {
                ContentType = request.TheFile.ContentType,
                FileName = uploadFileName,
                FileExtension = theExtension,
                FilePath = uploadFileres.FileAddress,
                FileSize = request.TheFile.Length,
                MinioBucket = uploadFileres.BucketName
            });


            var res = await _chat.PrivateChatSocketIO(new PrivateChatSocketIORequest { commandName = request.commandName, data = new Data { attachmentId = attachRes.AttachmentId, clientTime = request.data.clientTime, forwardedBy = request.data.forwardedBy, isRtl = request.data.isRtl, message = request.data.message, replyOf = request.data.replyOf, to = request.data.to } });
            res.attachmentId = attachRes?.AttachmentId;
            res.fileName = uploadFileName;
            return Ok(res);
        }

        [HttpPost("MessageToChatRoomIO")]
        public async Task<ActionResult> MessageToChatRoomIO(MessageToChatRoomIORequest request)
        {
            var res = await _chat.MessageToChatRoomIO(request);
            return Ok(res);
        }

        [HttpPost("MessageToChatRoomIOWithFile")]
        public async Task<ActionResult> MessageToChatRoomIOWithFile([FromForm] MessageToChatRoomIOWithFileRequest request)
        {
            var sizeLimit = 60 * 1024 * 1024;
            if (request.TheFile.Length > sizeLimit)
            {
                PrivateChatUploadFileResponse resError = new();
                resError.IsSuccess = false;

                resError.Message = $"File size limit {sizeLimit}";
                return StatusCode(((int)HttpStatusCode.NotAcceptable), resError);

            }

            var theName = request.TheFile.FileName;
            //var fromUserName = User.Identity.Name.ToLower();
            var serviceReq = new FileUploadPrivateChatRequest();
            //serviceReq.FromUserName = fromUserName;
            //serviceReq.ToUserName = request.ToUserName.ToLower();
            //serviceReq.Message = request.Message;
            serviceReq.FileName = request.TheFile.FileName.ToLower();
            serviceReq.FileExtension = Path.GetExtension(serviceReq.FileName).ToLower();


            var lastIndexOfDot = request.TheFile.FileName.LastIndexOf(".");
            var theExtension = request.TheFile.FileName.Substring(lastIndexOfDot);
            var theNameWithoutExtension = request.TheFile.FileName.Substring(0, lastIndexOfDot);

            DateTime currentTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
            //var newNameExtension =  $"{theNameWithoutExtension}_{unixTime}{theExtension}";

            var uploadFileName = $"{theNameWithoutExtension}_{unixTime}{theExtension}";

            var fileStream = request.TheFile.OpenReadStream();

            var uploadFileres = await _minIOService.UploadFileAsync(fileStream, uploadFileName, theExtension, request.TheFile.ContentType);

            var attachRes = _actions.PrivateChatUploadFile(new PrivateChatUploadFileRequest
            {
                ContentType = request.TheFile.ContentType,
                FileName = uploadFileName,
                FileExtension = theExtension,
                FilePath = uploadFileres.FileAddress,
                FileSize = request.TheFile.Length,
                MinioBucket = uploadFileres.BucketName
            });

            var res = await _chat.MessageToChatRoomIO(new MessageToChatRoomIORequest { commandName = request.commandName, data = new RoomData { attachmentId = attachRes.AttachmentId, chatRoomName = request.data.chatRoomName, clientTime = request.data.clientTime, forwardedBy = request.data.forwardedBy, isRtl = request.data.isRtl, message = request.data.message, replyOf = request.data.replyOf } });
            res.attachmentId = attachRes.AttachmentId;
            res.fileName = uploadFileName;
            return Ok(res);
        }

        [HttpPost("FileFromMinIO")]
        public async Task<IActionResult> FileFromMinIO()
        {
            string fileName = "Capture001_1737809147.png";
            var res = await _minIOService.DownloadFile(fileName);
            //var rss =  File(res, "application/octet-stream", fileName);

            //res.Position = 0;
            return File(res, "application/octet-stream", fileName); // returns a FileStreamResult
            //return Ok();
        }

        [HttpPost("CheckUserRoles")]
        public IActionResult CheckUserRoles()
        {
            var userName = User.Identity.Name.ToLower();
            var res = _actions.CheckUserRoles(new CheckUserRolesRequest { UserName = userName });
            return Ok(res);
        }

    }


}
