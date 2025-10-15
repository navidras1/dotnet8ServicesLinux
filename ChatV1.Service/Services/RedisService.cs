using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services
{
    public class RedisService
    {
        ///private IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connection;

        public RedisService(IConnectionMultiplexer connection)
        {
            this._connection = connection;
        }

        public void CheckIfUserIsOnline(string UserName)
        {
            var iclient = _connection.GetDatabase(0);
            var res =  iclient.StringGet(UserName);


        }

        
    }
}
