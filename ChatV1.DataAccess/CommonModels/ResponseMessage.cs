using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.CommonModels
{
    public class ResponseMessage
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "The operation is done successfully";
        public List<string> Warnings { get; set; } = new List<string>();
    }
}
