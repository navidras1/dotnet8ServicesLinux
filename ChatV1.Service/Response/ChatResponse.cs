using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class ChatResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "عملیات موفقیت آمیز بود.";
    }
}
