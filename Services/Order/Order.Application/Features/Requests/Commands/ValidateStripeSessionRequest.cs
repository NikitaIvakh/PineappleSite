using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands;

public sealed class ValidateStripeSessionRequest(ValidateStripeSessionDto validateStripeSessionDto)
    : IRequest<Result<OrderHeaderDto>>
{
    public ValidateStripeSessionDto ValidateStripeSessionDto { get; init; } = validateStripeSessionDto;
}