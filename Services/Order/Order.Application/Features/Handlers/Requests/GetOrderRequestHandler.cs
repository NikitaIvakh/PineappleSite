using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Order.Application.Features.Requests.Requests;
using Order.Application.Resources;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Handlers.Requests;

public sealed class GetOrderRequestHandler(
    IBaseRepository<OrderHeader> orderHeaderRepository,
    IMapper mapper,
    IMemoryCache memoryCache)
    : IRequestHandler<GetOrderRequest, Result<OrderHeaderDto>>
{
    private const string CacheKey = "cacheOrderListKey";

    public async Task<Result<OrderHeaderDto>> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out OrderHeaderDto? orderHeaderDto))
            {
                return new Result<OrderHeaderDto>()
                {
                    Data = orderHeaderDto,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("OrderSuccessfullyReceived", SuccessMessage.Culture),
                };
            }

            var orderHeader = await orderHeaderRepository.GetAll().Include(key => key.OrderDetails)
                .FirstOrDefaultAsync(key => key.OrderHeaderId == request.OrderId, cancellationToken);

            if (orderHeader is null)
            {
                return new Result<OrderHeaderDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("OrderNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("OrderNotFound", ErrorMessages.Culture) ?? string.Empty
                    ]
                };
            }

            return new Result<OrderHeaderDto>
            {
                StatusCode = (int)StatusCode.Ok,
                Data = mapper.Map<OrderHeaderDto>(orderHeader),
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("OrderSuccessfullyReceived", SuccessMessage.Culture),
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