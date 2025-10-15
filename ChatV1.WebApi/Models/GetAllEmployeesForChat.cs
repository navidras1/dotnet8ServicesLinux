namespace ChatV1.WebApi.Models
{
    public class GetAllEmployeesForChatForUser
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string UserName { get; set; }
        public string ToUserName { get; set; }

}
}
