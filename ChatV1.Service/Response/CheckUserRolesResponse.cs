using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class CheckUserRolesResponse: ResponseMessage
    {
        public List<string> Roles { get; set; }= new List<string>();
    }
}
