using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetUserChatRoomNotificationStatusResponse:ResponseMessage
    {
        public string RoomName { get; set; } = "";
        public bool? CanGetNotif {  get; set; }=null;
    }
}
