using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Request
{
    public class CreateRoomRequest
    {
        public IFormFile TheFile { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public string ChatRoomTypeName { get; set; } = "";
        public List<CreateRGUser> InviteeUserNames { get; set; }
        
        [SwaggerSchema(ReadOnly = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public string CreatorUserName { get; set; } = "";
        public string ChatRoomName { get; set; }
        public string? Description { get; set; } = null;
        public bool? IsChannel { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [System.Text.Json.Serialization.JsonIgnore]
        public long? LogoId { get; set; } = null;
    }
    public class CreateRGUser
    {
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
