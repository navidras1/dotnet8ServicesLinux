using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using ChatV1.WebApi.Models.AppSetting;
using ChatV1.Service.Services;
using Newtonsoft.Json;
using System.Text;
using ChatV1.Service.Model.ChatRoom;
namespace ChatV1.WebApi.BackgroundServices
{
    public class ChatRoomMessageLogBackgroundService : BackgroundService
    {

        private RabbitMQ.Client.IConnection _connection;
        private RabbitMQ.Client.IBasicConsumer _consumer;
        private ILogger<ChatRoomCreateBackgroundService> _logger;
        private IModel _channel;
        public IServiceProvider _services { get; }
        private IOptions<RabbitMqSettings> _rabbitMqSettings;
        private string chatRoomMessageQueue;

        public ChatRoomMessageLogBackgroundService(ILogger<ChatRoomCreateBackgroundService> logger, IServiceProvider services, IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _logger = logger;
            _services = services;
            _rabbitMqSettings = rabbitMqSettings;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://82.115.25.140:5672")
            };
            factory.UserName = _rabbitMqSettings.Value.UserName;
            factory.Password = _rabbitMqSettings.Value.Password;

            // create connection  
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            // create channel  

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {

                chatRoomMessageQueue = _rabbitMqSettings.Value.LogChatRoomMessageLocal;
            }
            else
            {
                chatRoomMessageQueue = _rabbitMqSettings.Value.LogChatRoomMessage;
            }

            _channel.QueueDeclare(chatRoomMessageQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _connection.ConnectionShutdown += _connection_ConnectionShutdown; ;



        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("before chatRoom");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += OnConsumerRecieved;
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;
            _channel.BasicConsume(chatRoomMessageQueue, true, consumer);
            return Task.CompletedTask;
        }
        private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"connection shut down {e.ReplyText}");
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

        private async void OnConsumerRecieved(object? sender, BasicDeliverEventArgs e)
        {
            _logger.LogInformation("on chat consumerrrrrrrrrrrrrr");
            using var scope = _services.CreateScope();
            var _actions = scope.ServiceProvider.GetRequiredService<IActions>();

            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var chatRoomLog = JsonConvert.DeserializeObject<ChatRoomMessage>(message);
            await _actions.SaveChatRoomMessage(chatRoomLog);
        }
    }
}

