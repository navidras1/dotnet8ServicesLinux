using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.Admin
{
    public class CreateChatRoomTypeRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; } = string.Empty;

        public int? MaxMember { get; set; }

        public bool? Istemp { get; set; }

        public bool? IsChannel { get; set; }
    }
}
