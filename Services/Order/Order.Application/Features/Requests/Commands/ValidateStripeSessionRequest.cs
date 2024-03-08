using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands
{
    public class ValidateStripeSessionRequest : IRequest<Result<OrderHeaderDto>>
    {
        public int OrderHeaderId { get; set; }
    }
}