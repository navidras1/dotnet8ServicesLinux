using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class RoomMembersResponse : ResponseMessage
    {
        public List<RoomMemberDetail> roomMemberDetails { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class RoomMemberDetail
    {
        public string userName { get; set; }
        public bool isAdmin { get; set; }  
        public string pkEmployee {  get; set; }
        public string fullName {  get; set; }
        public long? lastSeen { get; set; } = null;
    }
}
