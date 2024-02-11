using Newtonsoft.Json;
using PineappleSite.Infrastructure.RabbitMQ.Common;
using RabbitMQ.Client;
using System.Text;

namespace PineappleSite.Infrastructure.RabbitMQ.Events
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _userName = "guest";
            _password = "guest";
        }

        public void SendMessage(object baseMessage, string queueName)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            _connection = connectionFactory.CreateConnection();

            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, true, false, false, null);

            var json = JsonConvert.SerializeObject(baseMessage);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: queueName, null, body);
        }
    }
}