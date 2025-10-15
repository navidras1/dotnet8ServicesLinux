using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class DisableRoomRequest
    {
        public string RoomName {  get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string OwnerName { get; set; } = "";
    }
}
