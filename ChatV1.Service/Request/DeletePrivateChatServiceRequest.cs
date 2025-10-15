using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class DeletePrivateChatServiceRequest
    {
        public List<Guid> PrivateChatIds { get; set; }
    }
}
