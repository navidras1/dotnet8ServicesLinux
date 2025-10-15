using ChatV1.Service.Services.CallApi;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace ChatV1.WebApi.BackgroundServices
{
    public class RobotToken : BackgroundService
    {
        private readonly IGriffinAirAvation _griffinAirAvation;
        private ILogger<RobotToken> _logger;
        private readonly IConnectionMultiplexer _iconnection;
        private readonly IDistributedCache _distributedCache;
        private readonly ConnectionMultiplexer connection;
        private string EXPIRED_KEYS_CHANNEL = "__keyevent@0__:expired";
        private ISubscriber subscriber;

        public RobotToken(IGriffinAirAvation griffinAirAvation, ILogger<RobotToken> logger, IConnectionMultiplexer iconnection)
        {
            _griffinAirAvation = griffinAirAvation;
            _logger = logger;
            //_connection = connection;
            //string EXPIRED_KEYS_CHANNEL = "__keyevent@0__:expired";
            _iconnection = iconnection;
            var iclient = _iconnection.GetDatabase(0);
            iclient.StringSet("IGriffinToken_ToEx", "navid", TimeSpan.FromSeconds(60));
            //ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(host);



            Console.WriteLine("Listening for events...");
            connection = ConnectionMultiplexer.Connect("192.168.40.60:6379");
            var client = connection.GetDatabase(0);
            client.StringSet("GriffinToken_ToEx", "navid", TimeSpan.FromSeconds(10));


            

        }



        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            subscriber = connection.GetSubscriber();


            subscriber.Subscribe(EXPIRED_KEYS_CHANNEL, (channel, key) =>
                {
                    var mm = key.ToString();

                    Console.WriteLine($"EXPIRED: {key}");
                }
            );
            return Task.CompletedTask;
        }
    }
}
