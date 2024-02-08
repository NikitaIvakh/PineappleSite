using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Requests
{
    public class GetOrderListRequest : IRequest<Result<OrderHeaderDto>>
    {
        public string UserId { get; set; }
    }
}