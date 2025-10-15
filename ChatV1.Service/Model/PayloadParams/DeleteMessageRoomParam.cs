using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.PayloadParams
{
    public class DeleteMessageRoomParam
    {
        public List<Guid> ChatLogGuids {  get; set; }
        public string FromUserName { get; set; }
        public List<string> UserList { get; set; }
        public string RoomName {  get; set; }

    }
}
