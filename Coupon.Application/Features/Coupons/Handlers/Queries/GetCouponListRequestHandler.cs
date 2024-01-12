using AutoMapper;
using Coupon.Application.DTOs;
using Coupon.Application.Features.Coupons.Requests.Queries;
using MediatR;
using PineappleSite.Coupon.Core.Interfaces;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponListRequestHandler(ICouponRepository repository, IMapper mapper) : IRequestHandler<GetCouponListRequest, IEnumerable<CouponDto>>
    {
        private readonly ICouponRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CouponDto>> Handle(GetCouponListRequest request, CancellationToken cancellationToken)
        {
            var coupons = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }
    }
}