using MediatR;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Requests.Commands;

public sealed class UpdateOrderStatusRequest(UpdateOrderStatusDto updateOrderStatusDto)
    : IRequest<Result<OrderHeaderDto>>
{
    public UpdateOrderStatusDto UpdateOrderStatusDto { get; set; } = updateOrderStatusDto;
}