using ChatV1.DataAccess.CommonModels;
using ChatV1.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class HistoryChatOfUsersResponse :ResponseMessage
    {
        public Dictionary<string,List<ResponseV2_V2>> Result { get; set; } = new();
    }

    public class HistoryChatOfUsersResponseV2 : ResponseMessage
    {
        public Dictionary<string, HistoryMessage> Result { get; set; } = new();
    }
}
