using AutoMapper;
using MediatR;
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
using Order.Domain.Interfaces.Services;

namespace Order.Application.Features.Handlers.Requests
{
    public class GetOrderListRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IMapper mapper, IMemoryCache memoryCache, IUserService userService) : IRequestHandler<GetOrderListRequest, CollectionResult<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IUserService _userService = userService;

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
                        SuccessCode = (int)SuccessCode.Ok,
                        SuccessMessage = SuccessMessage.OrderList,
                        Data = _mapper.Map<IReadOnlyCollection<OrderHeaderDto>>(orderHeader),
                    };
                }

                string userId = request.UserId;
                var user = await _userService.GetUserAsync(userId);

                if (user.Data.Roles.Contains(StaticDetails.RoleAdmin))
                    orderHeader = _mapper.Map<List<OrderHeaderDto>>(await _orderHeaderRepository.GetAll().Include(key => key.OrderDetails).ToListAsync(cancellationToken));

                else
                    orderHeader = _mapper.Map<List<OrderHeaderDto>>(await _orderHeaderRepository.GetAll().Include(key => key.OrderDetails).Where(key => key.UserId == request.UserId).ToListAsync(cancellationToken));

                _memoryCache.Set(cacheKey, orderHeader);

                return new CollectionResult<OrderHeaderDto>
                {
                    Count = orderHeader.Count,
                    SuccessCode = (int)SuccessCode.Ok,
                    SuccessMessage = SuccessMessage.OrderList,
                    Data = _mapper.Map<IReadOnlyCollection<OrderHeaderDto>>(orderHeader),
                };
            }

            catch
            {
                _memoryCache.Remove(cacheKey);
                return new CollectionResult<OrderHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessages.InternalServerError]
                };
            }
        }
    }
}