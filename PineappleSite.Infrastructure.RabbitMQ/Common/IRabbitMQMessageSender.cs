namespace PineappleSite.Infrastructure.RabbitMQ.Common
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(object baseMessage, string queueName);
    }
}