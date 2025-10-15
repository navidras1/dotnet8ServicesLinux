using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.ChatRoom
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public bool IsRtl { get; set; }
        public long ClientTime { get; set; }
        public long? attachmentId { get; set; }
        public Guid? chatGuid { get; set; }
    }

    public class ChatRoomMessage
    {
        public List<ChatMessage> messages { get; set; }
        public string fromChatRoom { get; set; }
        public string fromUserName { get; set; }
        public int status { get; set; }
        public string forwardedBy {  get; set; }
        public string? replyOfGuid { get; set; }


    }
}
