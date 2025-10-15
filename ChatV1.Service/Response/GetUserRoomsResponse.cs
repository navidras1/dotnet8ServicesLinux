using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetUserRoomsResponse
    {
        public List<string> UserRooms { get; set; }
    }

    public class GetUserRoomsResponse2
    {
        public string RoomName { get; set; }
    }
}
