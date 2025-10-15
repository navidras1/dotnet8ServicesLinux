using ChatV1.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetUnreadMessagesResponse
    {
        public List<ChatLog> UnreadLogs { get; set; }=new List<ChatLog>();
        public int CountOfUnreadLogs { get; set; } 
        public string UserName { get; set; }
    }
}
