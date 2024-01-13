using AutoMapper;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class UpdateCouponRequestHandler(ICouponDbContext couponDbContext, IMapper mapper) : IRequestHandler<UpdateCouponRequest, Unit>
    {
        private readonly ICouponDbContext _repository = couponDbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateCouponRequest request, CancellationToken cancellationToken)
        {
            var coupon = await _repository.Coupons.FirstAsync(key => key.CouponId == request.Id, cancellationToken: cancellationToken);

            _mapper.Map(request.UpdateCoupon, coupon);
            _repository.Coupons.Update(coupon);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}