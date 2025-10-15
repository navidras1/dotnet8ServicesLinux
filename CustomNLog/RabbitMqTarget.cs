using Newtonsoft.Json;
using NLog.Config;
using NLog.Targets;
using NLog;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNLog
{
    [Target("RabbitMq")]
    public class RabbitMqTarget : TargetWithLayout
    {

        //IConfiguration _config;

        //public MyFirstTarget(IConfiguration config)
        //{
        //    this._config = config;
        //}

        //[RequiredParameter]
        //public string Host { get; set; }

        [ArrayParameter(typeof(TargetPropertyWithContext), "contextproperty")]
        public virtual IList<TargetPropertyWithContext> ContextProperties { get; } = new List<TargetPropertyWithContext>();
        string RabbitMqAddress = AppSettings.Configuration["RabbitMq:Address"];
        string RabbitMqUserName = AppSettings.Configuration["RabbitMq:UserName"];
        string RabbitMqPassword = AppSettings.Configuration["RabbitMq:Password"];
        private RabbitMQ.Client.IConnection _connection;
        private RabbitMQ.Client.IBasicConsumer _consumer;
        private IModel _channel;

        protected override void Write(LogEventInfo logEvents)
        {



            //base.Write(logEvents);
            try
            {
                string logMessage = this.Layout.Render(logEvents);
                //SendTheMessageToRemoteHost(this.Host, logMessage);
                Dictionary<string, object> properties = new Dictionary<string, object>();

                foreach (var context in ContextProperties)
                {
                    var tmpObj = context.RenderValue(logEvents);
                    properties.Add(context.Name, tmpObj);
                    //logModel.ClientIp = context.RenderValue()

                }
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(properties));

                _channel.BasicPublish("", LogQueue, null, body);
                //if (!File.Exists("RabbitLogFile.txt"))
                //{
                //    File.Create("RabbitLogFile.txt").Close();
                //}
                //File.AppendAllLines("RabbitLogFile.txt",  [JsonConvert.SerializeObject(properties)]);
            }
            catch (Exception)
            {

                //throw;
            }
        }
        string LogQueue = "";
        protected override void InitializeTarget()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqAddress)
                //Uri = new Uri("amqp://192.168.40.60:5672")
            };
            factory.UserName = RabbitMqUserName;
            factory.Password = RabbitMqPassword;

            // create connection  
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                LogQueue = AppSettings.Configuration["RabbitMq:LogMicroservice"];
            }
            else
            {
                LogQueue = AppSettings.Configuration["RabbitMq:LogMicroservice"];
            }
            // create channel  
            _channel.QueueDeclare(LogQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        private void SendTheMessageToRemoteHost(string host, string message)
        {
            Console.WriteLine($"naviiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiid {host} {message}");
        }
    }
}
