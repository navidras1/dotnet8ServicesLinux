using ChatV1.DataAccess.CommonModels;
using ChatV1.Service.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.ChatRoom
{
    public class UserChatRoomWithCountOfUnreadsResponseV2
    {
        public string roomName {  get; set; }
        public string lastMessage {  get; set; }
        public DateTimeOffset lastMessageDate { get; set; }
        public int countOfUnreadMessage { get; set; }
        public string senderUserName {  get; set; }
        public long senderUserId {  get; set; }
        
    }

    public class UsersChatRoomsWithCountOfUnreadsResponseV2 : ResponseMessage 
    {
        public List<UserChatRoomWithCountOfUnreadsResponseV2> UserChatRoomsWithCountOfUnreads { get; set; } = [];
        public int Total {  get; set; }=0;
    }

    public class UserChatRoomWithCountOfUnreadsResponseV3
    {
        public string roomName {  get; set; }
        public string roomType { get; set; }
        public string lastMessage {  get; set; }
        public long createDate { get; set; }
        public int countOfUnreadMessage { get; set; }
        public string fromUserName {  get; set; }
        public long senderUserId {  get; set; }
        public string userName { get; set; }

        public string toUserName { get; set; }

        public Guid lastMessageId { get; set; }
        public GetFileDetailResponse attachment { get; set; } = null;


    }

    public class UsersChatRoomsWithCountOfUnreadsResponseV3 : ResponseMessage 
    {
        public List<UserChatRoomWithCountOfUnreadsResponseV3> UserChatRoomsWithCountOfUnreads { get; set; } = [];
    }

    public class UsersChatRoomsWithCountOfUnreadsResponseV4 : ResponseMessage 
    {
        public List<Dictionary<string, object>> UserChatRoomsWithCountOfUnreads { get; set; } = [];
    }



}
