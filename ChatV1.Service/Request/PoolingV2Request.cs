using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class PoolingV2Request
    {
        public long FromTimeStamp {  get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string ToUserName { get; set; } = "";
    }
}
