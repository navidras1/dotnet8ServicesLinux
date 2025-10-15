using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class MakeMessageReadBeforeGuidRequest
    {
        public string FromUserName {  get; set; }
        public string ToUserName { get; set; }
        public Guid MessageGuid { get; set; }
    }
}
