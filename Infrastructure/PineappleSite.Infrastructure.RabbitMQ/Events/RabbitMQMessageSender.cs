using Newtonsoft.Json;
using PineappleSite.Infrastructure.RabbitMQ.Common;
using RabbitMQ.Client;
using System.Text;

namespace PineappleSite.Infrastructure.RabbitMQ.Events;

public sealed class RabbitMqMessageSender : IRabbitMqMessageSender
{
    private const string HostName = "localhost";
    private const string UserName = "guest";
    private const string Password = "guest";
    private IConnection _connection = null!;

    public bool SendMessage(object baseMessage, string? queueName)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = HostName,
            UserName = UserName,
            Password = Password
        };

        _connection = connectionFactory.CreateConnection();

        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: queueName, true, false, false, null);

        var json = JsonConvert.SerializeObject(baseMessage);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();
        properties.ContentType = "application/json";

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body);

        return true;
    }
}