using AutoMapper;
using MediatR;
using Order.Application.Features.Requests.Commands;
using Order.Application.Resources;
using Order.Application.Utility;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;
using Stripe;

namespace Order.Application.Features.Handlers.Commands
{
    public class UpdateOrderStatusRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IMapper mapper) : IRequestHandler<UpdateOrderStatusRequest, Result<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;

        private readonly IMapper _mapper = mapper;

        public async Task<Result<OrderHeaderDto>> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                OrderHeader orderHeader = _orderHeaderRepository.GetAll().First(key => key.OrderHeaderId == request.OrderHeaderId);

                if (orderHeader is not null)
                {
                    if (request.NewStatus == StaticDetails.Status_Cancelled)
                    {
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId,
                        };

                        var service = new RefundService();
                        Refund refund = service.Create(options);
                    }

                    orderHeader.Status = request.NewStatus;
                    await _orderHeaderRepository.UpdateAsync(orderHeader);
                }

                return new Result<OrderHeaderDto>
                {
                    Data = _mapper.Map<OrderHeaderDto>(orderHeader),
                    SuccessMessage = "Статус заказа умпешно обновлен",
                };
            }

            catch (Exception exception)
            {
                return new Result<OrderHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message }
                };
            }
        }
    }
}