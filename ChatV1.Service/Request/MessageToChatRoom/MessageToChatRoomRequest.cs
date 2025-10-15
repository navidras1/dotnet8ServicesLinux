using ChatV1.Service.Request.PrivateChatSocketIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request.MessageToChatRoom
{

    public class RoomData
    {
        public string chatRoomName { get; set; }
        public string message { get; set; }
        public bool isRtl { get; set; }
        public long clientTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public long? attachmentId { get; set; } = null;
        public string? forwardedBy { get; set; } = null;
        public object? replyOf { get; set; } = null;
    }
    public class MessageToChatRoomIORequest
    {
        public string commandName { get; set; } = "messageToChatRoom";
        public RoomData data { get; set; }
    }
}
