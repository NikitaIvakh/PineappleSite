using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands
{
    public class RabbitMQSendRequest : IRequest<Result<bool>>
    {
        public CartDto CartDto { get; set; }
    }
}