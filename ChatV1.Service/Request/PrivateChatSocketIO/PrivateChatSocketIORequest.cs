using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request.PrivateChatSocketIO
{
    public class Data
    {
        public string to { get; set; }
        public string message { get; set; }
        public bool isRtl { get; set; }
        public long clientTime { get; set; }
        public long? attachmentId { get; set; }
        public string? forwardedBy { get; set; } = null;
        public object? replyOf { get; set; } = null;
    }



    public class PrivateChatSocketIORequest
    {
        public string commandName { get; set; }
        public Data data { get; set; }
    }


}
