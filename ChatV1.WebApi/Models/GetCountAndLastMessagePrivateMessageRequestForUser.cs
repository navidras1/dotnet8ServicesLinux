namespace ChatV1.WebApi.Models
{
    public class GetCountAndLastMessagePrivateMessageRequestForUser
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetCountAndLastMessagePrivateMessageRequestForUserV2
    {
        public long FromTimeStamp {  get; set; }
    }

}
