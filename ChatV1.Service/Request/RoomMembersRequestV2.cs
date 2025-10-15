using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class RoomMembersRequestV2
    {
        public RoomDetailV2 RoomDetail { get; set; }
        public string UserName { get; set; }
    }

    public class RoomDetailV2
    {
        public long RoomId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
