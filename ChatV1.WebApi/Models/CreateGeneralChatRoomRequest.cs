namespace ChatV1.WebApi.Models
{
    public class CreateGeneralChatRoomRequest
    {
        public string InviteeUserNames { get; set; }
        public string ChatRoomName { get; set;}
        public string Description { get; set;}
    }
}
