using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class CreateChannelGroupRoomRequest
    {
        public string InviteeUserNames { get; set; }
        
        public string ChatRoomName { get; set; }
        public string? Description { get; set; }
        public bool? IsChannel { get; set; }
    }

    
}
