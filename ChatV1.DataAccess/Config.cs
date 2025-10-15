using ChatV1.DataAccess.Context;
using ChatV1.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess
{
    public static class Config
    {
        public static void ConfigChatDataAccess(this IServiceCollection services, IConfiguration config)
        {
            
            var connectionString = config["ConnectionStrings:DefaultConnection"];
            var postgresql = config["ConnectionStrings:PosgresConnection"];
            //var connectionString = @"Server=192.168.40.57; Database=ChatV1; User Id=sa; Password=Sep@44902051; MultipleActiveResultSets=true; TrustServerCertificate=True";
            
            //services.AddDbContextFactory<ChatV1Context>(options => options.UseSqlServer(connectionString));
            services.AddDbContextFactory<ChatV1Context>(options => options.UseNpgsql(postgresql));
            
            services.AddScoped(typeof(IChatV1Repository<>), typeof(ChatV1Repository<>));

        }

    }
}
