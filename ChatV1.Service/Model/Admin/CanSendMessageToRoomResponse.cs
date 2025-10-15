using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class CanSendMessageToRoomResponse : ResponseMessage
    {
        public bool CanSend {  get; set; } = false;
        public string RoomType { get; set; }
    }
}
