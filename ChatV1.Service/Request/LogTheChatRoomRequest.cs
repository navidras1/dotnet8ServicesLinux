using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class LogTheChatRoomRequest
    {
        public string message { get; set; }
        public string fromChatRoom { get; set; }
        public string fromUserName { get; set; }
        public long createDate { get; set; }
        public int status { get; set; }
    }
}
