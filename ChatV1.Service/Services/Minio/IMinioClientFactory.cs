using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services.Minio
{
    public interface IMinioClientFactory
    {
        IMinioClient CreateClient();
        IMinioClient CreateClient(string name);
    }
}
