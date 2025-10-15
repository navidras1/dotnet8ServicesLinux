using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetListOfRoomsWithUnreadMessagesRequestV3
    {
        public string? UserName { get; set; } = null;
    }
}
