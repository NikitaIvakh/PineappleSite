namespace PineappleSite.Infrastructure.RabbitMQ.Common
{
    public interface IRabbitMQMessageSender
    {
        bool SendMessage(object baseMessage, string? queueName);
    }
}