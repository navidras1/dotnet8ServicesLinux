using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetContactListResponse:ResponseMessage
    {
        public List<Dictionary<string , object>> Contacts { get; set; }
        public int Total { get; set; }
    }
}
