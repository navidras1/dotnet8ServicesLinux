using ChatV1.DataAccess.CommonModels;
using ChatV1.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class HistoryMessageOfUsersResponse
    {
        public List<ResponseV2>? ChatLogs { get; set; } 
        public int TotalSize { get; set; }
    }

    public class HistoryMessageOfUsersResponseV2 : ResponseMessage
    {
        public List<ResponseV2_V2>? ChatLogs { get; set; } 
        public int TotalSize { get; set; }
    }


}
