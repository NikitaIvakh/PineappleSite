using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Order.Application.Features.Requests.Commands;
using Order.Application.Resources;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;
using Stripe.Checkout;

namespace Order.Application.Features.Handlers.Commands;

public sealed class CreateStripeSessionRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IMemoryCache memoryCache)
    : IRequestHandler<CreateStripeSessionRequest, Result<StripeRequestDto>>
{
    private const string CacheKey = "cacheOrderCreateKey";
    
    public async Task<Result<StripeRequestDto>> Handle(CreateStripeSessionRequest request,
        CancellationToken cancellationToken)
    {
        var service = new SessionService();

        try
        {
            var options = new SessionCreateOptions
            {
                LineItems = [],
                CancelUrl = request.StripeRequest.CancelUrl,
                SuccessUrl = request.StripeRequest.ApprovedUrl,

                Mode = "payment",
            };

            var discountObj = new List<SessionDiscountOptions>()
            {
                new()
                {
                    Coupon = request.StripeRequest.OrderHeader?.CouponCode,
                }
            };

            foreach (var item in request.StripeRequest.OrderHeader?.OrderDetails!)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "byn",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product!.Name,
                            Description = item.Product.Description,
                        },
                    },

                    Quantity = item.Count,
                };

                options.LineItems.Add(sessionLineItem);
            }

            if (request.StripeRequest.OrderHeader.Discount > 0)
            {
                options.Discounts = discountObj;
            }

            var session = service.Create(options);
            request.StripeRequest.StripeSessionUrl = session.Url;

            var orderHeader = await orderHeaderRepository.GetAll().FirstOrDefaultAsync(key => key.OrderHeaderId == request.StripeRequest.OrderHeader.OrderHeaderId, cancellationToken);
            orderHeader!.StripeSessionId = session.Id;
            request.StripeRequest.StripeSessionId = session.Id;

            await orderHeaderRepository.UpdateAsync(orderHeader);
            memoryCache.Remove(CacheKey);

            return new Result<StripeRequestDto>
            {
                Data = request.StripeRequest,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage = SuccessMessage.ThePaymentWasSuccessful,
            };
        }

        catch (Exception ex)
        {
            return new Result<StripeRequestDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}