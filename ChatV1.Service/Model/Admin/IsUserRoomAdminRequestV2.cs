using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class IsUserRoomAdminRequestV2
    {
        public long ChatRoomId { get; set; }
        public string userName { get; set; }
    }
}
