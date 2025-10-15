using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class PrivateChatMessageToUsersRequest
    {

        public string Message { get; set; }
        public bool IsRtl { get; set; }
        public List<string> ToUserNames { get; set; }
        public int NumberOfMessages { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string FromUserName { get; set; } = "griffin";
        public long? attachmentId { get; set; } = null;
    }
}
