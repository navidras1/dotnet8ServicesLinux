using ChatV1.DataAccess.Models;
using ChatV1.Service.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace ChatV1.WebApi.BackgroundServices
{
    public class ChatLogBackgroundService : BackgroundService
    {
        //private IRabbitMQConsumer _rabbitMQConsumer;
        private RabbitMQ.Client.IConnection _connection;
        private RabbitMQ.Client.IBasicConsumer _consumer;
        private ILogger<ChatLogBackgroundService> _logger;
        private IModel _channel;
        public IServiceProvider _services { get; }



        public ChatLogBackgroundService(ILogger<ChatLogBackgroundService> logger, IServiceProvider services)
        {

            _logger = logger;
            _services = services;
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://82.115.25.140:5672")
            };
            factory.UserName = "admin";
            factory.Password = "adminpassword";

            // create connection  
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            var privateChat = "";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                privateChat = "privateChatOnlocalServer";
            }
            else
            {
                privateChat = "privateChatV2";
            }
            // create channel  
            _channel.QueueDeclare(privateChat,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;



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
            var privateChat = "";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                privateChat = "privateChatOnlocalServer";
            }
            else
            {
                privateChat = "privateChatV2";
            }


            _channel.BasicConsume(privateChat, true, consumer);
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        private async void OnConsumerRecieved(object sender, BasicDeliverEventArgs e)
        {

            //var _actions = 
            using (var scope = _services.CreateAsyncScope())
            {
                var _actions = scope.ServiceProvider.GetRequiredService<IActions>();


                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"{message}");

                var chatLog = JsonConvert.DeserializeObject<Service.Request.LogTheChatRequest>(message);
                var res = DateTimeOffset.FromUnixTimeMilliseconds(131321);
                //Convert.ToDateTime()
                await _actions.LogTheChat(chatLog);
                //_channel.BasicReject(e.DeliveryTag, true);


                //await Task.Run(() => _actions.LogTheChat(chatLog));



            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"connection shut down {e.ReplyText}");
        }
        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"consumer cancelled {e.ConsumerTags}");
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"consumer unregistered {e.ConsumerTags}");
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInformation($"consumer registered {e.ConsumerTags}");
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"consumer shutdown {e.ReplyText}");
        }

    }
}
