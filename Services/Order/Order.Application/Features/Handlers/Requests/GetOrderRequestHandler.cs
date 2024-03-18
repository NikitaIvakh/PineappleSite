using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Order.Application.Features.Requests.Requests;
using Order.Application.Resources;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;

namespace Order.Application.Features.Handlers.Requests
{
    public class GetOrderRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<GetOrderRequest, Result<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheOrderListKey";

        public async Task<Result<OrderHeaderDto>> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out OrderHeader? orderHeader))
                {
                    return new Result<OrderHeaderDto>
                    {
                        SuccessMessage = "Заказ успешно получен",
                        Data = _mapper.Map<OrderHeaderDto>(orderHeader),
                    };
                }

                else
                {
                    orderHeader = await _orderHeaderRepository.GetAll().Include(key => key.OrderDetails).FirstOrDefaultAsync(key => key.OrderHeaderId == request.OrderId, cancellationToken);

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
                        _memoryCache.Set(cacheKey, orderHeader);

                        return new Result<OrderHeaderDto>
                        {
                            SuccessMessage = "Заказ успешно получен",
                            Data = _mapper.Map<OrderHeaderDto>(orderHeader),
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _memoryCache.Remove(cacheKey);
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