using MediatR;
using Microsoft.Extensions.Configuration;
using PineappleSite.Infrastructure.RabbitMQ.Common;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands
{
    public class RabbitMQSendRequestHandler(IRabbitMQMessageSender messageSender, IConfiguration configuration) : IRequestHandler<RabbitMQSendRequest, Result<bool>>
    {
        private readonly IRabbitMQMessageSender _messageSender = messageSender;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Result<bool>> Handle(RabbitMQSendRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var message = _messageSender.SendMessage(request.CartDto, _configuration.GetValue<string>("TopicAndQueueNames:RabbitMQSendRequestHandler"));

                if (message is false)
                {
                    return new Result<bool>
                    {
                        ErrorMessage = ErrorMessages.InternalServerError,
                        ErrorCode = (int)ErrorCodes.InternalServerError,
                    };
                }

                else
                {
                    return new Result<bool>
                    {
                        Data = true,
                        SuccessMessage = "Сообщение успешно отправлено",
                    };
                }
            }

            catch (Exception exception)
            {
                return new Result<bool>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message }
                };
            }
        }
    }
}