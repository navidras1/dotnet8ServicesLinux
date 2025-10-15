using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services.Minio
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///   Add default minio client with default configuration
        /// </summary>
        public static IServiceCollection AddMinio(this IServiceCollection services)
        {
            return services.AddMinio(_ =>
            {
            });
        }

        /// <summary>
        ///   Configure minio client using Uri
        /// </summary>
        /// <example>s3://accessKey:secretKey@localhost:9000/region</example>
        public static IServiceCollection AddMinio(
          this IServiceCollection services,
          Uri url,
          Action<ChatMinioOptions>? configure = null)
        {
            return services.AddMinio(Options.DefaultName, url, configure);
        }

        /// <summary>
        ///   Configure default minio client
        /// </summary>
        public static IServiceCollection AddMinio(this IServiceCollection services, Action<ChatMinioOptions> configure)
        {
            return services.AddMinio(Options.DefaultName, configure);
        }

        /// <summary>
        ///   Configure named minio client using Uri
        /// </summary>
        /// <example>s3://accessKey:secretKey@localhost:9000/region</example>
        public static IServiceCollection AddMinio(
          this IServiceCollection services,
          string name,
          Uri url,
          Action<ChatMinioOptions>? configure = null)
        {
            return services.AddMinio(name, options =>
            {
                var credentials = url.UserInfo.Split(':');

                if (credentials.Length != 2)
                {
                    throw new InvalidOperationException(
                      $"Invalid credentials format: {url.UserInfo}. s3://accessKey:secretKey@endpoint expected");
                }

                options.Endpoint = url.Authority;
                options.AccessKey = credentials[0];
                options.SecretKey = credentials[1];
                options.Region = url.AbsolutePath.TrimStart('/');

                configure?.Invoke(options);
            });
        }

        /// <summary>
        ///   Configure named minio client
        /// </summary>
        public static IServiceCollection AddMinio(
          this IServiceCollection services,
          string name,
          Action<ChatMinioOptions> configure)
        {
            services.Configure(name, configure);
            services.TryAddSingleton<IMinioClientFactory, MinioClientFactory>();
            services.TryAddSingleton(sp => sp.GetRequiredService<IMinioClientFactory>().CreateClient(name));

            return services;
        }
    }
}
