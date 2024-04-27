using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands;

public sealed class UpdateOrderStatusRequest(int orderHeaderId, string newStatus) : IRequest<Result<OrderHeaderDto>>
{
    public int OrderHeaderId { get; init; } = orderHeaderId;

    public string NewStatus { get; init; } = newStatus;
}