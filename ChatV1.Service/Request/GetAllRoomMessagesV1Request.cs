using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetAllRoomMessagesV1Request
    {
        public string UserName { get; set; }
        public string RoomName { get; set; }
        public long ChatRoomId { get; set; }
    }
}
