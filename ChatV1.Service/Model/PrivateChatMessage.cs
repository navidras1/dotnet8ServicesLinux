using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model
{
    public class PrivateChatData
    {
        public string to { get; set; }
        public string message { get; set; }
        public bool isRtl { get; set; }
        public long clientTime { get; set; }=DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public long? attachmentId { get; set; } = null;
    }

    public class PrivateChatMessage
    {
        public string commandName { get; set; } = "privateChat";
        public PrivateChatData data { get; set; }
    }
}
