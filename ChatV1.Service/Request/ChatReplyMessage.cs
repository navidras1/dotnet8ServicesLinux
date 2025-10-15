using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class ChatReplyMessage
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Msg { get; set; }
        public string FileType { get; set; }
        public Guid? Guid { get; set; }
    }
}
