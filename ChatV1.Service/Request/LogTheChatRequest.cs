namespace ChatV1.Service.Request
{
    public class LogTheChatRequest
    {
        public long Id { get; set; }

        public int FromEmpId { get; set; }

        public int ToEmPid { get; set; }

        public List<ChatMessage> Messages { get; set; } = null!;

        public int ChatStatusId { get; set; }

        public string CreateDate { get; set; }

        public Guid ChatGuid { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public long? AttachmentId { get; set; }
        public long? ClientDateTime {  get; set; }
        public string ForwardedBy {  get; set; }
        public string? ReplyOfGuid { get; set; }
    }

    public class ChatMessage
    {
        public string Message { get; set; }
        public bool? IsRtl {  set; get; }
    }
}