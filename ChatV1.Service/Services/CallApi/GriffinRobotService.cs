using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatV1.Service.Services.CallApi
{
    public interface IGriffinRobotService
    {
        string GriffinToken { set;get; }
        Task Subscriber();
    }

    public class GriffinRobotService : IGriffinRobotService
    {
        private readonly IConnectionMultiplexer _connection;
        private ISubscriber _subscriber;
        private string EXPIRED_KEYS_CHANNEL = "__keyevent@0__:expired";
        private IGriffinAirAvation _griffinAirAvation;

        public GriffinRobotService(IConnectionMultiplexer connection, IGriffinAirAvation griffinAirAvation)
        {
            _connection = connection;
            _griffinAirAvation = griffinAirAvation;
        }

        public string GriffinToken {get; set;}

        public async Task Init()
        {
            var iclient = _connection.GetDatabase(0);
            var token =  iclient.StringGet("IGriffinToken_ToEx");
            if (token.IsNull)
            {
                var resRequestToken = await _griffinAirAvation.RequestToken();
                var resToken = resRequestToken.token;
                GriffinToken= resToken;
              
                iclient.StringSet("IGriffinToken_ToEx", resToken, TimeSpan.FromSeconds(20));
            }


        }

        public async Task Subscriber()
        {
            await Init();
            _subscriber = _connection.GetSubscriber();

            _subscriber.Subscribe(EXPIRED_KEYS_CHANNEL, async (channel, key) =>
            {
                var mm = key.ToString();
                if(key.ToString()== "IGriffinToken_ToEx")
                {
                    await Init();
                }

                Console.WriteLine($"EXPIRED: {key}");
            });
        }

    }
}
