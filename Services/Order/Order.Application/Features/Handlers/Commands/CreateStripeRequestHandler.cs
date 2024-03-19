using AutoMapper;
using MediatR;
using Order.Application.Features.Requests.Commands;
using Order.Application.Resources;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;
using Order.Infrastructure;
using Stripe.Checkout;

namespace Order.Application.Features.Handlers.Commands
{
    public class CreateStripeRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository) : IRequestHandler<CreateStripeRequest, Result<StripeRequestDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;

        public async Task<Result<StripeRequestDto>> Handle(CreateStripeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = request.StripeRequest.ApprovedUrl,
                    CancelUrl = request.StripeRequest.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),

                    Mode = "payment",
                };

                var discountObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions
                    {
                        Coupon = request.StripeRequest.OrderHeader.CouponCode,
                    }
                };

                foreach (var item in request.StripeRequest.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "byn",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                                Description = item.Product.Description,
                            },
                        },

                        Quantity = item.Count,
                    };

                    options.LineItems.Add(sessionLineItem);
                };

                if (request.StripeRequest.OrderHeader.Discount > 0)
                {
                    options.Discounts = discountObj;
                }

                var service = new SessionService();
                Session session = service.Create(options);
                request.StripeRequest.StripeSessionUrl = session.Url;

                OrderHeader orderHeader = _orderHeaderRepository.GetAll().First(key => key.OrderHeaderId == request.StripeRequest.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                request.StripeRequest.StripeSessionId = session.Id;

                await _orderHeaderRepository.UpdateAsync(orderHeader);

                return new Result<StripeRequestDto>
                {
                    Data = request.StripeRequest,
                    SuccessCode = (int)SuccessCode.Ok,
                    SuccessMessage = SuccessMessage.ThePaymentWasSuccessful,
                };
            }

            catch
            {
                return new Result<StripeRequestDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessages.InternalServerError]
                };
            }
        }
    }
}