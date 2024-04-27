using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Requests;

public sealed class GetOrderRequest(int orderId) : IRequest<Result<OrderHeaderDto>>
{
    public int OrderId { get; init; } = orderId;
}