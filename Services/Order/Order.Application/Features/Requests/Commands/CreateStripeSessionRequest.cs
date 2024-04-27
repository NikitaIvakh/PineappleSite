using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands;

public sealed class CreateStripeSessionRequest(StripeRequestDto stripeRequest) : IRequest<Result<StripeRequestDto>>
{
    public StripeRequestDto StripeRequest { get; init; } = stripeRequest;
}