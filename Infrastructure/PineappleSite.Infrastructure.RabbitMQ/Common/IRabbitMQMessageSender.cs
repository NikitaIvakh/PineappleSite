namespace PineappleSite.Infrastructure.RabbitMQ.Common;

public interface IRabbitMqMessageSender
{
    bool SendMessage(object baseMessage, string? queueName);
}