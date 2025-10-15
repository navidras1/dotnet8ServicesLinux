using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class CountOfChatRoomUnreadMessageRequest
    {
        public string UserName {  get; set; }
        public string ChatRoomName { get; set; }
    }
}
