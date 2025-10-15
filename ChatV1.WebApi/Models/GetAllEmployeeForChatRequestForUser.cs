namespace ChatV1.WebApi.Models
{
    public class GetAllEmployeeForChatRequestForUser
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string userName { get; set; }
        
    }
}
