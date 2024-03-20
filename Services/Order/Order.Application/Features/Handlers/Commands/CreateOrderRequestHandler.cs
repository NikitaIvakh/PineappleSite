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

namespace Order.Application.Features.Handlers.Commands
{
    public class CreateOrderRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IMapper mapper, IOrderValidator orderValidator) : IRequestHandler<CreateOrderRequest, Result<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IOrderValidator _orderValidator = orderValidator;

        public async Task<Result<OrderHeaderDto>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validResult = await _orderValidator.ValidateAsync(request.CartDto.CartHeader, cancellationToken);

                if (!validResult.IsValid)
                {
                    var exceptionsError = new Dictionary<string, List<string>>
                    {
                        {"Name", validResult.Errors.Select(key => key.ErrorMessage).ToList() },
                        {"Email", validResult.Errors.Select(key => key.ErrorMessage).ToList() },
                        {"PhoneNumber", validResult.Errors.Select(key => key.ErrorMessage).ToList() },
                    };

                    foreach (var error in exceptionsError)
                    {
                        if (exceptionsError.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<OrderHeaderDto>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessages.OrderNotCreated,
                                ErrorCode = (int)ErrorCodes.OrderNotCreated,
                            };
                        }
                    }

                    return new Result<OrderHeaderDto>
                    {
                        ErrorMessage = ErrorMessages.OrderNotCreated,
                        ErrorCode = (int)ErrorCodes.OrderNotCreated,
                        ValidationErrors = validResult.Errors.Select(key => key.ErrorMessage).ToList()
                    };
                }

                else
                {
                    OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(request.CartDto.CartHeader);
                    orderHeaderDto.OrderTime = DateTime.UtcNow;
                    orderHeaderDto.Status = StaticDetails.Status_Pending;
                    orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(request.CartDto.CartDetails);
                    orderHeaderDto.OrderTotal = Math.Round(orderHeaderDto.OrderTotal, 2);

                    OrderHeader orderCreated = await _orderHeaderRepository.CreateAsync(_mapper.Map<OrderHeader>(orderHeaderDto));

                    orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;

                    return new Result<OrderHeaderDto>
                    {
                        Data = orderHeaderDto,
                        SuccessCode = (int)SuccessCode.Created,
                        SuccessMessage = SuccessMessage.OrderSuccessfullyCreated,
                    };
                }
            }

            catch (Exception exception)
            {
                return new Result<OrderHeaderDto>
                {
                    ValidationErrors = [exception.Message],
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorMessages.InternalServerError,
                };
            }
        }
    }
}