using MediatR;
using Microsoft.Extensions.Configuration;
using PineappleSite.Infrastructure.RabbitMQ.Common;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands;

public sealed class RabbitMqSendRequestHandler(IRabbitMQMessageSender messageSender, IConfiguration configuration)
    : IRequestHandler<RabbitMqSendRequest, Result<bool>>
{
    public Task<Result<bool>> Handle(RabbitMqSendRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var message = messageSender.SendMessage(request.CartDto,
                configuration.GetValue<string>("TopicAndQueueNames:RabbitMQSendRequestHandler"));

            if (message is false)
            {
                return Task.FromResult(new Result<bool>
                {
                    StatusCode = (int)StatusCode.InternalServerError,
                    ErrorMessage =
                        ErrorMessages.ResourceManager.GetString("InternalServerError", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("InternalServerError", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                });
            }

            return Task.FromResult(new Result<bool>
            {
                Data = true,
                StatusCode = (int)StatusCode.Created,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("MessageSentSuccessfully", SuccessMessage.Culture),
            });
        }

        catch (Exception ex)
        {
            return Task.FromResult(new Result<bool>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            });
        }
    }
}