using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Repository
{
    public class GetAllFromSPWithOutputResponseViewModel: ResponseMessage
    {
        public List<Dictionary<string, object>> Result { get; set; }= new List<Dictionary<string, object>>();
        public Dictionary<string, object> OutPuts { get; set; }= new Dictionary<string, object>();
    }
}
