using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class UpdateChatRoomMessagesToReadRequest
    {
        public string ChatRoomName {  get; set; }
        public string UserName { get; set;}
    }
}
