using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class CheckIfUserCanDeleteChatLogRequest
    {
        public string RoomName { get; set; }
        public string UserName { get; set; }
        public List<Guid> MessageGuids { get; set; }
    }
}
