using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands;

public sealed class CreateOrderRequest(CartDto cartDto) : IRequest<Result<OrderHeaderDto>>
{
    public CartDto CartDto { get; init; } = cartDto;
}