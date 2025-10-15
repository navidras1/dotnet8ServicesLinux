using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class CreateChatRoomRequest
    {
        public string? ChatRoomName { get; set; }

        public bool? IsActive { get; set; }

        public string? Description { get; set; } = string.Empty;

        public long? ChatRoomTypeId { get; set; }

        public string? CreatorUserName { get; set; }
    }
}
