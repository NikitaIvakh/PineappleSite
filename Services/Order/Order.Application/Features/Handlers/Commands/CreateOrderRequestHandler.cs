using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Order.Application.Features.Requests.Commands;
using Order.Application.Resources;
using Order.Application.Utility;
using Order.Application.Validators;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Handlers.Commands;

public sealed class CreateOrderRequestHandler(
    IBaseRepository<OrderHeader> orderHeaderRepository,
    IMapper mapper,
    IMemoryCache memoryCache,
    OrderValidator orderValidator) : IRequestHandler<CreateOrderRequest, Result<OrderHeaderDto>>
{
    private const string CacheKey = "cacheOrderCreateKey";

    public async Task<Result<OrderHeaderDto>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validResult = await orderValidator.ValidateAsync(request.CartDto.CartHeader, cancellationToken);

            if (!validResult.IsValid)
            {
                var exceptionsError = new Dictionary<string, List<string>>
                {
                    { "Name", validResult.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "Email", validResult.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "PhoneNumber", validResult.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "Address", validResult.Errors.Select(key => key.ErrorMessage).ToList() },
                };

                foreach (var error in exceptionsError)
                {
                    if (exceptionsError.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new Result<OrderHeaderDto>
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessages.ResourceManager.GetString("OrderNotCreated", ErrorMessages.Culture),
                        };
                    }
                }

                return new Result<OrderHeaderDto>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = validResult.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("OrderNotCreated", ErrorMessages.Culture),
                };
            }

            var orderHeaderDto = mapper.Map<OrderHeaderDto>(request.CartDto.CartHeader);
            orderHeaderDto.OrderTime = DateTime.UtcNow;
            orderHeaderDto.Status = StaticDetails.StatusPending;
            orderHeaderDto.OrderDetails = mapper.Map<IEnumerable<OrderDetailsDto>>(request.CartDto.CartDetails);
            orderHeaderDto.OrderTotal = Math.Round(orderHeaderDto.OrderTotal, 2);

            var orderCreated = await orderHeaderRepository.CreateAsync(mapper.Map<OrderHeader>(orderHeaderDto));
            orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;

            memoryCache.Remove(CacheKey);

            return new Result<OrderHeaderDto>
            {
                Data = orderHeaderDto,
                StatusCode = (int)StatusCode.Created,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("OrderSuccessfullyCreated", SuccessMessage.Culture),
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