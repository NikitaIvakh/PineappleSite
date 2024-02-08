﻿using AutoMapper;
using MediatR;
using Order.Application.Features.Requests.Commands;
using Order.Application.Resources;
using Order.Application.Utility;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;
using Order.Infrastructure;
using Stripe;
using Stripe.Checkout;

namespace Order.Application.Features.Handlers.Commands
{
    public class ValidateStripeSessionRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, ApplicationDbContext applicationDbContext, IMapper mapper) : IRequestHandler<ValidateStripeSessionRequest, Result<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<OrderHeaderDto>> Handle(ValidateStripeSessionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                OrderHeader orderHeader = _orderHeaderRepository.GetAll().First(key => key.OrderHeaderId == request.OrderHeaderId);
                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = StaticDetails.Status_Approved;
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                }

                return new Result<OrderHeaderDto>
                {
                    SuccessMessage = "Оплата прошла успешно!",
                    Data = _mapper.Map<OrderHeaderDto>(orderHeader),
                };
            }

            catch (Exception exception)
            {
                return new Result<OrderHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message },
                };
            }
        }
    }
}