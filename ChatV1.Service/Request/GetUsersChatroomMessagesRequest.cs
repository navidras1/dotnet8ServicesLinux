using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetUsersChatroomMessagesRequest
    {
        public string UserName { get; set; }
        public string RoomName { get; set;}
        public int PageNumber { get; set; }
        public int PageSize { get; set; } 


    }
}
