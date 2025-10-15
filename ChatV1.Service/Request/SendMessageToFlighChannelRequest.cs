using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class SendMessageToFlighChannelRequest
    {
        public bool isRtl { get; set; }
        public long? attachmentId { get; set; }
        public string message { get; set; }
    }
}
