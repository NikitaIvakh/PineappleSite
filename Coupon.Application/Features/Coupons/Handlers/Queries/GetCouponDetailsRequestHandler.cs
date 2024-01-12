using AutoMapper;
using Coupon.Application.DTOs;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponDetailsRequestHandler(ICouponDbContext repository, IMapper mapper) : IRequestHandler<GetCouponDetailsRequest, CouponDto>
    {
        private readonly ICouponDbContext _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<CouponDto> Handle(GetCouponDetailsRequest request, CancellationToken cancellationToken)
        {
            var coupon = await _repository.Coupons.FirstAsync(key => key.CouponId == request.Id, cancellationToken);
            return _mapper.Map<CouponDto>(coupon);
        }
    }
}