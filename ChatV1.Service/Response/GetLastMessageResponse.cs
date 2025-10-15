using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Response
{
    public class GetLastMessageResponse
    {
        public string Message { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
