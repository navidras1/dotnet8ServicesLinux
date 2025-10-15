using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class CheckOfflineActionsResponse:ResponseMessage
    {
        public List<string> Result { get; set; } = new();
    }
}
