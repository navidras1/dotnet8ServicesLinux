using ChatV1.Service.Services;
using ChatV1.WebApi.Models.AppSetting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatV1.WebApi.BackgroundServices
{
    public class RobotChatRoomBackgroundService : BackgroundService
    {
        private RabbitMQ.Client.IConnection _connection;
        private RabbitMQ.Client.IBasicConsumer _consumer;
        private ILogger<RobotChatRoomBackgroundService> _logger;
        private IModel _channel;
        public IServiceProvider _services { get; }
        private IOptions<RabbitMqSettings> _rabbitMqSettings;
        private string RobotQueue;

        public RobotChatRoomBackgroundService(ILogger<RobotChatRoomBackgroundService> logger, IServiceProvider services, IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _logger = logger;
            _services = services;
            _rabbitMqSettings = rabbitMqSettings;
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://192.168.40.60:5672")
            };
            factory.UserName = "admin";
            factory.Password = "Mr@li77aa@@";

            // create connection  
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                RobotQueue = "privateChatOnlocalServer";
            }
            else
            {
                RobotQueue = "privateChatV2";
            }
            // create channel  
            _channel.QueueDeclare(RobotQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _connection.ConnectionShutdown += _connection_ConnectionShutdown;



        }



        private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"connection shut down {e.ReplyText}");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("before");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += OnConsumerRecieved;
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            var chatRoomLog = "";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                chatRoomLog = "chatRoomLogLocal";
            }
            else
            {
                chatRoomLog = "chatRoomLog";
            }


            _channel.BasicConsume(chatRoomLog, true, consumer);
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
        private void OnConsumerConsumerCancelled(object? sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"consumer cancelled {e.ConsumerTags}");
        }

        private void OnConsumerUnregistered(object? sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"consumer unregistered {e.ConsumerTags}");
        }

        private void OnConsumerRegistered(object? sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"consumer registered {e.ConsumerTags}");
        }

        private void OnConsumerShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"consumer shutdown {e.ReplyText}");
        }

        private void OnConsumerRecieved(object? sender, BasicDeliverEventArgs e)
        {
            using var scope = _services.CreateScope();
            var _actions = scope.ServiceProvider.GetRequiredService<IActions>();

            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var chatRoomLog = JsonConvert.DeserializeObject<Service.Request.LogTheChatRoomRequest>(message);
            _actions.LogTheChatRoom(chatRoomLog);
        }
    }
}
