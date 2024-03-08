using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Requests
{
    public class GetOrderRequest : IRequest<Result<OrderHeaderDto>>
    {
        public int OrderId { get; set; }
    }
}