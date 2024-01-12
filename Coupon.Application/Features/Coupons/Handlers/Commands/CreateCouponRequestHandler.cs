using AutoMapper;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Interfaces;
using Coupon.Core.Entities;
using MediatR;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class CreateCouponRequestHandler(ICouponDbContext couponDbContext, IMapper mapper) : IRequestHandler<CreateCouponRequest, int>
    {
        private readonly ICouponDbContext _repository = couponDbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<int> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<CouponEntity>(request.CreateCouponDto);

            await _repository.Coupons.AddAsync(coupon, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return coupon.CouponId;
        }
    }
}