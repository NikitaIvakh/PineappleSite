using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Order.Application.Features.Requests.Requests;
using Order.Application.Resources;
using Order.Application.Utility;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.ResultOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Order.Application.Features.Handlers.Requests
{
    public class GetOrderListRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<GetOrderListRequest, CollectionResult<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheOrderListKey";

        public async Task<CollectionResult<OrderHeaderDto>> Handle(GetOrderListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out List<OrderHeaderDto>? orderHeader))
                {
                    return new CollectionResult<OrderHeaderDto>
                    {
                        Count = orderHeader.Count,
                        SuccessMessage = "Список заказов",
                        Data = _mapper.Map<IReadOnlyCollection<OrderHeaderDto>>(orderHeader),
                    };
                }

                var userRole = _httpContextAccessor.HttpContext.User.IsInRole(StaticDetails.RoleAdmin);

                if (userRole is true)
                    orderHeader = _mapper.Map<List<OrderHeaderDto>>(await _orderHeaderRepository.GetAll().Include(key => key.OrderDetails).ToListAsync(cancellationToken));

                else
                    orderHeader = _mapper.Map<List<OrderHeaderDto>>(await _orderHeaderRepository.GetAll().Include(key => key.OrderDetails).Where(key => key.UserId == request.UserId).ToListAsync(cancellationToken));

                return new CollectionResult<OrderHeaderDto>
                {
                    Count = orderHeader.Count,
                    SuccessMessage = "Список заказов",
                    Data = _mapper.Map<IReadOnlyCollection<OrderHeaderDto>>(orderHeader),
                };
            }

            catch (Exception exception)
            {
                _memoryCache.Remove(cacheKey);
                return new CollectionResult<OrderHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message }
                };
            }
        }
    }
}