using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response.GetAllEmployeeForChat
{
    //public class GetAllEmployeeForChatResponseItemViewModel
    //{
    //    public int pkEmployee { get; set; }
    //    public string fullName { get; set; }
    //    public string userName { get; set; }
    //    public int noUnreadMessage { get; set; }
    //    public object picture { get; set; }
    //}

    public class GetAllEmployeeRolesForChatResponseItemViewModel
    {
        public int pkEmployee { get; set; }
        public string fullName { get; set; }
    }

    public class GetAllEmployeeForChatResponse
    {
        public List<Dictionary<string, object>> getAllEmployeeForChatResponseItemViewModels { get; set; }
        public List<Dictionary<string, object>> getAllEmployeeRolesForChatResponseItemViewModels { get; set; }
        public int total { get; set; }
        public int pageSize { get; set; }
        public bool success { get; set; }
        public List<object> responseMessages { get; set; }
    }
}
