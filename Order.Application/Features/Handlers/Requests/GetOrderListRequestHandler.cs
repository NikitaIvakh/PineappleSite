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

namespace Order.Application.Features.Handlers.Requests
{
    public class GetOrderListRequestHandler(IBaseRepository<OrderHeader> orderHeaderRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : IRequestHandler<GetOrderListRequest, CollectionResult<OrderHeaderDto>>
    {
        private readonly IBaseRepository<OrderHeader> _orderHeaderRepository = orderHeaderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;

        public async Task<CollectionResult<OrderHeaderDto>> Handle(GetOrderListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<OrderHeader> orderHeader;
                var userRole = _httpContextAccessor.HttpContext.User.IsInRole(StaticDetails.RoleAdmin);

                if (userRole is true)
                    orderHeader = _orderHeaderRepository.GetAll().Include(key => key.OrderDetails).OrderBy(key => key.OrderHeaderId).ToList();

                else
                    orderHeader = _orderHeaderRepository.GetAll().Include(key => key.OrderDetails).Where(key => key.UserId == request.UserId).OrderBy(key => key.OrderHeaderId).ToList();

                return new CollectionResult<OrderHeaderDto>
                {
                    Count = orderHeader.Count(),
                    SuccessMessage = "Список заказов",
                    Data = _mapper.Map<List<OrderHeaderDto>>(orderHeader),
                };
            }

            catch (Exception exception)
            {
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