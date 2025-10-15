using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class AddUsersToRoomRequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string? requester {  get; set; }=null;
        public string RoomName { get; set; }
        public List<CreateRGUser> InviteeUserNames { get; set; }
    }

    public class AddUserMembers
    {
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
