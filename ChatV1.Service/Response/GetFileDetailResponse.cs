using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetFileDetailResponse
    {
        public long id {  get; set; }
        public string name { get; set; }
        public long size { get; set; }
        public string type { get; set; }
        public string? contentType { get; set; }
    }
}
