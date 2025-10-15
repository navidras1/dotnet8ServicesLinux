using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetUserChatRoomNotificationStatusRequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserName { get; set; } = "";
        public string RoomName {  get; set; }
    }
}
