using ChatV1.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class ChatLogWithRoomId
    {
        public ChatLog chatLog { get; set; }
        public long ChatRoomId { get; set; } 
    }
}
