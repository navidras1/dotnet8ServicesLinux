using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetAllEmployeeForChatRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string UserName { get; set; }

        public string ToUserName { get; set; }
        public string NotUsersIn { get; set; }

        public string MasterUserName { get; set; }
    }
}
