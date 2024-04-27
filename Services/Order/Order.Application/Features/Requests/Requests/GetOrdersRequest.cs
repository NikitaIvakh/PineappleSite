using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Requests;

public class GetOrdersRequest(string userId) : IRequest<CollectionResult<OrderHeaderDto>>
{
    public string UserId { get; init; } = userId;
}