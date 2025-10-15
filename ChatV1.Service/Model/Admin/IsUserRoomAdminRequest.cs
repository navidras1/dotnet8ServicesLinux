using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class IsUserRoomAdminRequest
    {
        public string ChatRoomName { get; set; }
        public string userName { get; set; }
    }
}
