using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class SetLastEmpLastSeenRequest
    {
        public string UserName { get; set; }
        public string LastSeenDate { get; set; }
    }
}
