using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response.GetEployeeDetails
{
    public class Result
    {
        public int PkEmployee { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    public class GetEployeeDetailsResponse
    {
        public List<Dictionary<string,object>> result { get; set; }= new List<Dictionary<string, object>>();
        public bool success { get; set; }
        public List<object> responseMessages { get; set; }
    }

}
