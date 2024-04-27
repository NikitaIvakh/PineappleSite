using AutoMapper;
using MediatR;
using Order.Application.Features.Requests.Requests;
using Order.Application.Resources;
using Order.Application.Utility;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Order.Domain.Interfaces.Services;

namespace Order.Application.Features.Handlers.Requests;

public sealed class GetOrdersRequestHandler(
    IBaseRepository<OrderHeader> orderHeaderRepository,
    IMapper mapper,
    IMemoryCache memoryCache,
    IUserService userService) : IRequestHandler<GetOrdersRequest, CollectionResult<OrderHeaderDto>>
{
    private const string CacheKey = "cacheOrderListKey";

    public async Task<CollectionResult<OrderHeaderDto>> Handle(GetOrdersRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out IReadOnlyCollection<OrderHeaderDto>? orderHeaderDtos))
            {
                return new CollectionResult<OrderHeaderDto>()
                {
                    Data = orderHeaderDtos,
                    Count = orderHeaderDtos!.Count,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage = SuccessMessage.ResourceManager.GetString("OrderList", SuccessMessage.Culture),
                };
            }

            IReadOnlyCollection<OrderHeaderDto>? orderHeader;
            var userId = request.UserId;
            var user = await userService.GetUserAsync(userId);

            if (user?.Data is null)
            {
                return new CollectionResult<OrderHeaderDto>()
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("UserNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                        [ErrorMessages.ResourceManager.GetString("UserNotFound", ErrorMessages.Culture) ?? string.Empty]
                };
            }

            if (user.Data!.Role.Contains(StaticDetails.RoleAdmin))
                orderHeader = mapper.Map<IReadOnlyCollection<OrderHeaderDto>>(await orderHeaderRepository
                    .GetAll()
                    .Include(key => key.OrderDetails)
                    .ToListAsync(cancellationToken));

            else
                orderHeader = mapper.Map<IReadOnlyCollection<OrderHeaderDto>>(await orderHeaderRepository
                    .GetAll()
                    .Include(key => key.OrderDetails)
                    .Where(key => key.UserId == request.UserId)
                    .ToListAsync(cancellationToken));

            memoryCache.Remove(CacheKey);

            return new CollectionResult<OrderHeaderDto>
            {
                Data = orderHeader,
                Count = orderHeader.Count,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage = SuccessMessage.ResourceManager.GetString("OrderList", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<OrderHeaderDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}