using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Repository
{
    public class FnSpRequest
    {
        public string FNSpName { get; set; }
        public List<ServiceOperatorParameter> Parameters { get; set; }= new List<ServiceOperatorParameter>();
    }

    public class ServiceOperatorParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
        public bool? IsOutPut { get; set; }

        public UserDefinedDataTable UserDefinedDataTable { get; set; }
    }



    public class UserDefinedDataTable
    {
        public string Name { get; set; }
        public List<string[]> Values { get; set; }

    }
}
