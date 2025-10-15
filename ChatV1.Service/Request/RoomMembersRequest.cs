using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class RoomMembersRequest
    {
        public RoomDetail RoomDetail {  get; set; }
        public string UserName {  get; set; }
    }
    public class RoomDetail
    {
        public string RoomName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
