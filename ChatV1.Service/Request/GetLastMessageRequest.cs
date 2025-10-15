using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetLastMessageRequest
    {
        public string fromUser {  get; set; }   
        public string toUser { get; set; }

    }
}
