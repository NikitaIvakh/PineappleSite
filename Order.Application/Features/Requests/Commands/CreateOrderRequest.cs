using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands
{
    public class CreateOrderRequest : IRequest<Result<OrderHeaderDto>>
    {
        public CartDto CartDto { get; set; }
    }
}