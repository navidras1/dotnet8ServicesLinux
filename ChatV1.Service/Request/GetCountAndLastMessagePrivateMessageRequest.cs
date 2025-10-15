using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class GetCountAndLastMessagePrivateMessageRequest
    {
        public string ToUserName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
    public class GetCountAndLastMessagePrivateMessageRequestV2
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string ToUserName { get; set; } = "";
        public long FromTimeStamp {  get; set; }
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }

    }
}
