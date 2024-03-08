using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands
{
    public class CreateStripeRequest : IRequest<Result<StripeRequestDto>>
    {
        public StripeRequestDto StripeRequest { get; set; }
    }
}