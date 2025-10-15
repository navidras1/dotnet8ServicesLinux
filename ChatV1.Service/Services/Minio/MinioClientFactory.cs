using Microsoft.Extensions.Options;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services.Minio
{
    public class MinioClientFactory : IMinioClientFactory
    {
        private readonly IOptionsMonitor<ChatMinioOptions> optionsMonitor;

        public MinioClientFactory(IOptionsMonitor<ChatMinioOptions> optionsMonitor)
        {
            this.optionsMonitor = optionsMonitor;
        }

        public IMinioClient CreateClient()
        {
            return CreateClient(Options.DefaultName);
        }

        public IMinioClient CreateClient(string name)
        {
            var options = optionsMonitor.Get(name);


            var client = new MinioClient()

              .WithEndpoint(options.Endpoint)
              .WithCredentials(options.AccessKey, options.SecretKey)
              
              .WithSessionToken(options.SessionToken);


            if (!string.IsNullOrEmpty(options.Region))
            {
                client.WithRegion(options.Region);
            }
            //client.BucketExistsAsync
            

            options.Configure?.Invoke(client);

            return client.Build();
        }
    }
}
