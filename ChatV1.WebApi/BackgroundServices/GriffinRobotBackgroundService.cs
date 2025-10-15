using ChatV1.Service.Services.CallApi;
using ChatV1.WebApi.Controllers;
using StackExchange.Redis;

namespace ChatV1.WebApi.BackgroundServices
{
    public class GriffinRobotBackgroundService : BackgroundService
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly ILogger<GriffinRobotBackgroundService> _logger;
        private ISubscriber _subscriber;
        private string EXPIRED_KEYS_CHANNEL = "__keyevent@0__:expired";
        private IGriffinAirAvation _griffinAirAvation;

        public GriffinRobotBackgroundService(IConnectionMultiplexer connection, IGriffinAirAvation griffinAirAvation , ILogger<GriffinRobotBackgroundService> logger)
        {
            _connection = connection;
            _griffinAirAvation = griffinAirAvation;
            _logger = logger;
        }

        public async Task Init()
        {
            var iclient = _connection.GetDatabase(0);
            var token = iclient.StringGet("IGriffinToken_ToEx");
            if (token.IsNull)
            {
                var resRequestToken = await _griffinAirAvation.RequestToken();
                var resToken = resRequestToken.token;

                iclient.StringSet("IGriffinToken_ToEx", resToken, TimeSpan.FromMinutes(36000));
                
            }


        }
        public async Task SetToken()
        {
            var iclient = _connection.GetDatabase(0);
            var resRequestToken = await _griffinAirAvation.RequestToken();
            var resToken = resRequestToken.token;

            iclient.StringSet("IGriffinToken_ToEx", resToken, TimeSpan.FromMinutes(36000));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Init();
            _subscriber = _connection.GetSubscriber();

            _subscriber.Subscribe(EXPIRED_KEYS_CHANNEL, async (channel, key) =>
            {
                var mm = key.ToString();
                if (key.ToString() == "IGriffinToken_ToEx")
                {
                    await Init();
                }

                Console.WriteLine($"EXPIRED: {key}");
            });

            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    await SetToken();
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex.Message);
                }
                await Task.Delay(30000, stoppingToken);
            }
            //return Task.CompletedTask;
        }
    }
}
