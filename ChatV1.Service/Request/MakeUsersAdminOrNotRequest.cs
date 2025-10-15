using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class MakeUsersAdminOrNotRequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string? requester { get; set; } = null;
        public string RoomName { get; set; }
        public string? NewOwoner {  get; set; }= null;
        public List<CreateRGUser> InviteeUserNames { get; set; }
    }
}
