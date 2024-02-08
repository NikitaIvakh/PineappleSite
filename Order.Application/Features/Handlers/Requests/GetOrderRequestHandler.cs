using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Features.Requests.Requests;
using Order.Application.Resources;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Handlers.Requests
{
    public class GetOrderRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IMapper mapper) : IRequestHandler<GetOrderRequest, Result<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<OrderHeaderDto>> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                OrderHeader? orderHeader = _orderHeaderRepository.GetAll().Include(key => key.OrderDetails).FirstOrDefault(key => key.OrderHeaderId == request.OrderId);

                if (orderHeader is null)
                {
                    return new Result<OrderHeaderDto>
                    {
                        ErrorMessage = ErrorMessages.OrderNotFound,
                        ErrorCode = (int)ErrorCodes.OrderNotFound,
                    };
                }

                else
                {
                    return new Result<OrderHeaderDto>
                    {
                        SuccessMessage = "Заказ успешно получен",
                        Data = _mapper.Map<OrderHeaderDto>(orderHeader),
                    };
                }
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