using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model
{
    public class MessageToChatRoomData
    {
        public string chatRoomName { get; set; }
        public string message { get; set; }
        public bool isRtl { get; set; }
        public long clientTime { get; set; }
        public long? attachmentId { get; set; }
    }

    public class MessageToChatRoom
    {
        public string commandName { get; set; } = "messageToChatRoom";
        public MessageToChatRoomData data { get; set; }
    }

}
