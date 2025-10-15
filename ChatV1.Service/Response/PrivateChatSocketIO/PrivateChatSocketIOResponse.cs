using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response.PrivateChatSocketIO
{
    public class Result
    {
        public string messageGUID { get; set; }
        public long serverTime { get; set; }
    }

    public class PrivateChatSocketIOResponse: ResponseMessage
    {
        public string status { get; set; }
        public long? attachmentId { get; set; } = null;
        public string fileName { get; set; } = null;
        public Result result { get; set; }
    }

}
