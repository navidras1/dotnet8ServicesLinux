using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class HistoryMessageOfUsersRequest
    {
        public string fromUser { get; set; }
        public string toUser { get; set; } = "";

        public string requestor { get; set; } = "";

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class HistoryMessageOfUsersRequestV2
    {
        public string fromUser { get; set; }
        public string toUser { get; set; }

        public string requestor { get; set; } = "";

        public Guid? latestId { get; set; } 
        public bool isForward { get; set; } 
        public int perPage { get; set; }
        public bool? isIncluded { get; set; } = false;

    }



}
