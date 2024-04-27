using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Order.Application.Features.Requests.Commands;
using Order.Application.Resources;
using Order.Application.Utility;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;
using Stripe;

namespace Order.Application.Features.Handlers.Commands;

public sealed class UpdateOrderStatusRequestHandler(
    IBaseRepository<OrderHeader> orderHeaderRepository,
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<UpdateOrderStatusRequest, Result<OrderHeaderDto>>
{
    private const string CacheKey = "cacheOrderCreateKey";

    public async Task<Result<OrderHeaderDto>> Handle(UpdateOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var orderHeader = await orderHeaderRepository.GetAll()
                .FirstOrDefaultAsync(key => key.OrderHeaderId == request.OrderHeaderId, cancellationToken);

            if (orderHeader is null)
            {
                return new Result<OrderHeaderDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage =
                        ErrorMessages.ResourceManager.GetString("OrderHeaderNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("OrderHeaderNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }

            if (request.NewStatus == StaticDetails.StatusCancelled)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId,
                };

                var service = new RefundService();
                var refund = service.Create(options);
            }

            orderHeader.Status = request.NewStatus;
            await orderHeaderRepository.UpdateAsync(orderHeader);

            memoryCache.Remove(CacheKey);

            return new Result<OrderHeaderDto>
            {
                StatusCode = (int)StatusCode.Modify,
                Data = mapper.Map<OrderHeaderDto>(orderHeader),
                SuccessMessage = SuccessMessage.OrderStatusSuccessfullyUpdated,
            };
        }

        catch (Exception ex)
        {
            return new Result<OrderHeaderDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}