using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class DeleteChatOfflineRequest
    {
        public Guid MessageGuid { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string FromUsername { get; set; } = "";
        public string Username { get; set; }
    }
}
