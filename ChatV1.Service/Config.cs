using ChatV1.DataAccess;
using ChatV1.DataAccess.Repository;
using ChatV1.Service.Services;
//using ChatV1.Service.Services.Logs;
using ChatV1.Service.Services.Minio;
using ChatV1.Service.Services.SocketIo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using System;

namespace ChatV1.Service
{
    public static class Config
    {
        public static void ConfigChatService(this IServiceCollection services, IConfiguration config)
        {
            services.ConfigChatDataAccess(config);
            //services.AddScoped<IActions, Actions>();
            services.AddScoped(typeof(IActions), typeof(Actions));
            //services.AddScoped(typeof(ILogResponse), typeof(LogResponse));
            services.AddScoped(typeof(IChat), typeof(Chat));
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped(typeof(ISeed), typeof(Seed));
            services.AddKeyedSingleton<IMinIOService, ChatMinIOService>("ChatMinIO");
            services.AddSingleton<IRobotChat,RobotChat>();
            //services.AddMinio()

            //services.AddSingleton<IMinioClient>(sp =>
            //{
            //    try
            //    {

            //        return new MinioClient()
            //            .WithEndpoint(config["ChatMinIO:url"])
            //            .WithCredentials(config["ChatMinIO:accessKey"], config["ChatMinIO:secretKey"])
            //            .Build();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Failed to initialize MinioClient: {ex.Message}");
            //        throw;
            //    }
            //});
            //services.AddMinio(p =>
            //{
            //    p.WithEndpoint(config["ChatMinIO:url"])
            //            .WithCredentials(config["ChatMinIO:accessKey"], config["ChatMinIO:secretKey"])
            //            .Build();
            //});

            

            

            //services.Configure<ChatMinioOptions>(p =>
            //{
            //    p.SecretKey = config["ChatMinIO:secretKey"];
            //    p.AccessKey = config["ChatMinIO:accessKey"];
            //    p.Endpoint = config["ChatMinIO:url"];

            //});

            //services.AddMinio("ChatMinIO", p => {
            //    p.SecretKey = config["ChatMinIO:secretKey"];
            //    p.AccessKey = config["ChatMinIO:accessKey"];
            //    p.Endpoint = config["ChatMinIO:url"];

            //});

            //services.AddMinio(options => {
            //    options.WithEndpoint("");
            //    options.WithCredentials("", "");
            //    //options.defa

            
            //});


        }
    }
}