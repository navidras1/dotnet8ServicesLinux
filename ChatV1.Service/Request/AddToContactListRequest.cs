using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class AddToContactListRequest
    {
        public string OwnerUserName {get;set;}
        public List<string> Usernames { get;set;}= new List<string>();
    }
}
