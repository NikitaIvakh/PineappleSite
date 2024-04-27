using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands;

public sealed class ValidateStripeSessionRequest(int orderHeaderId) : IRequest<Result<OrderHeaderDto>>
{
    public int OrderHeaderId { get; init; } = orderHeaderId;
}