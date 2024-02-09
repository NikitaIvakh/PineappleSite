using MediatR;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Requests
{
    public class GetOrderListRequest : IRequest<CollectionResult<OrderHeaderDto>>
    {
        public string UserId { get; set; }
    }
}