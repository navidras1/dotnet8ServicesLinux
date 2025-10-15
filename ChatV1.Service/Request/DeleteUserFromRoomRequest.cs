using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class DeleteUserFromRoomRequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string Requestor { get; set; } = "";
        public string RoomName { get; set; }
        public List<string> UserNames { get; set; }
    }
}
