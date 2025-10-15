using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class CanSendMessageToRoomResponseV2: ResponseMessage
    {
        public bool CanSend { get; set; } = false;
        public string RoomType { get; set; }
        public bool? CanGetPushNotification {  get; set; }
    }
}
