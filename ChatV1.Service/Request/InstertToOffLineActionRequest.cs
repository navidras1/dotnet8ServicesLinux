using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class InstertToOffLineActionRequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string FromUserName { get; set; } = "";
        public string ToUserName { get; set; } 
        public Guid MessageGuid { get; set; }
        public long ActionTypeId { get; set; }
        public string Action { get; set; }
        public bool IsOnline { get; set; }
    }
}
