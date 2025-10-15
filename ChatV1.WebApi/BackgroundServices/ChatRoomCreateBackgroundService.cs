using ChatV1.DataAccess.Models;
using ChatV1.Service.Services;
using ChatV1.WebApi.Models.AppSetting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace ChatV1.WebApi.BackgroundServices
{
    public class ChatRoomCreateBackgroundService : BackgroundService
    {
        private RabbitMQ.Client.IConnection _connection;
        private RabbitMQ.Client.IBasicConsumer _consumer;
        private ILogger<ChatRoomCreateBackgroundService> _logger;
        private IModel _channel;
        public IServiceProvider _services { get; }
        private IOptions<RabbitMqSettings> _settings;

        public ChatRoomCreateBackgroundService(ILogger<ChatRoomCreateBackgroundService> logger, IServiceProvider services, IOptions<RabbitMqSettings> settings)
        {
            _logger = logger;
            _services = services;
            _settings = settings;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://185.179.216.215:5672")
            };
            factory.UserName = "guest";
            factory.Password = "guest";

            // create connection  
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            // create channel  
            var chatRoomLog = "";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                chatRoomLog = "chatRoomLogLocal";
            }
            else
            {
                chatRoomLog = "chatRoomLog";
            }

            _channel.QueueDeclare(chatRoomLog,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _connection.ConnectionShutdown += _connection_ConnectionShutdown; ;



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
