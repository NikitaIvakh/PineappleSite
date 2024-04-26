using MediatR;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Requests.Commands;

public class RabbitMqSendRequest(CartDto cartDto) : IRequest<Result<bool>>
{
    public CartDto CartDto { get; init; } = cartDto;
}