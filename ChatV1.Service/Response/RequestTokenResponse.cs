using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class RequestTokenResponse
    {
        public string token { get; set; }
        public string tokenType { get; set; }
        public int expiresIn { get; set; }
        public string userName { get; set; }
        public DateTime issued { get; set; }
        public DateTime expires { get; set; }
        public int pkEmployeeSession { get; set; }
        public string securityStamp { get; set; }
        public int result { get; set; }
        public string msg { get; set; }
        public bool success { get; set; }
        public List<object> responseMessages { get; set; }
    }

}
