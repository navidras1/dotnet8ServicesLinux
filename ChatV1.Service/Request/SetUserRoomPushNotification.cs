using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class SetUserRoomPushNotificationRequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserName { get; set; } = "";
        public string RoomName { get; set; }

        public bool CanGetPushNotification { get; set; }
    }
}
