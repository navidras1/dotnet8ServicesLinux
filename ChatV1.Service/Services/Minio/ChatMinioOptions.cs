using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services.Minio
{
    public class ChatMinioOptions
    {
        public string Endpoint { get; set; } = default!;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string SessionToken { get; set; } = string.Empty;

        internal Action<IMinioClient>? Configure { get; private set; }

        public void ConfigureClient(Action<IMinioClient> configure)
        {
            Configure = configure;
        }
    }
}
