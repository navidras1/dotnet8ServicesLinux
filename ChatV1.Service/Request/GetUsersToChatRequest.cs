using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetUsersToChatRequest
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string userName { get; set; } = "";
        public string toUserName { get; set; } = "";
        public string notUsersIn { get; set; } = "";
    }
}
