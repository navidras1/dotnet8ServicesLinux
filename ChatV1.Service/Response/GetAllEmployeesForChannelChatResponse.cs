using ChatV1.DataAccess.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetAllEmployeesForChannelChatResponse
    {
        public List<Dictionary<string, string>> result { get; set; } = new();
        public bool success {  get; set; }
        public List<string> responseMessages { get; set; } = new();

    }
}
