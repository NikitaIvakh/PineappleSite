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

namespace Order.Application.Features.Handlers.Commands
{
    public class CreateOrderRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IMapper mapper) : IRequestHandler<CreateOrderRequest, Result<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<OrderHeaderDto>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(request.CartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = StaticDetails.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(request.CartDto.CartDetails);
                orderHeaderDto.OrderTotal = Math.Round(orderHeaderDto.OrderTotal, 2);

                OrderHeader orderCreated = await _orderHeaderRepository.CreateAsync(_mapper.Map<OrderHeader>(orderHeaderDto));

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;

                return new Result<OrderHeaderDto>
                {
                    Data = orderHeaderDto,
                    SuccessMessage = "Заказ успешно создан",
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