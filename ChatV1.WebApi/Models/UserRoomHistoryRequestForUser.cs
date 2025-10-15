namespace ChatV1.WebApi.Models
{
    public class UserRoomHistoryRequestForUser
    {
        public string ChatRoomName {  get; set; }
        public Guid? Chatguid { get; set; }
    }
}
