using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request.MessageToChatRoom
{
    public class RoomDataWithFile
    {
        public string chatRoomName { get; set; }
        public string message { get; set; }
        public bool isRtl { get; set; }
        public long clientTime { get; set; }
        public long? attachmentId { get; set; }
        public string? forwardedBy { get; set; } = null;
        public object? replyOf { get; set; } = null;
    }

    public class MessageToChatRoomIOWithFileRequest
    {
        public string commandName { get; set; }
        public IFormFile TheFile { get; set; }
        public RoomDataWithFile data { get; set; }
    }
}
