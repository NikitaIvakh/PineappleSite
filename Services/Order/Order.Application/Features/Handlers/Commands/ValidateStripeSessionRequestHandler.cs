using AutoMapper;
using MediatR;
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
using Stripe.Checkout;

namespace Order.Application.Features.Handlers.Commands;

public sealed class ValidateStripeSessionRequestHandler(
    IBaseRepository<OrderHeader> orderHeaderRepository,
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<ValidateStripeSessionRequest, Result<OrderHeaderDto>>
{
    private const string CacheKey = "cacheOrderCreateKey";

    public async Task<Result<OrderHeaderDto>> Handle(ValidateStripeSessionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var orderHeader = orderHeaderRepository.GetAll().First(key => key.OrderHeaderId == request.ValidateStripeSessionDto.OrderHeaderId);
            var service = new SessionService();
            var session = service.Get(orderHeader.StripeSessionId);

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

            if (paymentIntent.Status != "succeeded")
            {
                return new Result<OrderHeaderDto>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("PaytmentError", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("PaytmentError", ErrorMessages.Culture) ?? string.Empty
                    ]
                };
            }

            orderHeader.PaymentIntentId = paymentIntent.Id;
            orderHeader.Status = StaticDetails.StatusApproved;
            await orderHeaderRepository.UpdateAsync(orderHeader);
            memoryCache.Remove(CacheKey);

            return new Result<OrderHeaderDto>
            {
                StatusCode = (int)StatusCode.Ok,
                Data = mapper.Map<OrderHeaderDto>(orderHeader),
                SuccessMessage = SuccessMessage.ThePaymentWasSuccessful,
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