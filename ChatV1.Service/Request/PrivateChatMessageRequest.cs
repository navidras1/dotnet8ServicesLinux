using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class PrivateChatMessageRequest
    {
        public string Message {  get; set; }
        public bool IsRtl { get; set; }
        public string ToUserName { get; set; }
        public int NumberOfMessages {  get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string FromUserName { get; set; } = "griffin";
    }
}
