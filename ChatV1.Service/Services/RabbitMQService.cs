using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Triangulate;

namespace ChatV1.Service.Services
{
    public class RabbitMQService
    {
        private IConfiguration _configuration;
        private ConnectionFactory factory;
        private IConnection _iConnection;



        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;

            factory = new ConnectionFactory {HostName = configuration["RabbitMq:Address"]};
            factory.UserName = "admin";
            factory.Password = "Mr@li77aa@@";

            _iConnection = factory.CreateConnection();

            _iConnection.CreateModel();

        }

        

    }
}
