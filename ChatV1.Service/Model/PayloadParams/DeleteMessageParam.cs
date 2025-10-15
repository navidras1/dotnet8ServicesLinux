using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Model.PayloadParams
{
    public class DeleteMessageParam
    {
        public string fromUserName { get; set; }
        public string toUserName { get; set; }
        public List<Guid> messageGuid { get; set; }
    }
}
