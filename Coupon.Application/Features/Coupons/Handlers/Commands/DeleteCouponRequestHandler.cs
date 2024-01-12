using AutoMapper;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Interfaces;
using MediatR;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class DeleteCouponRequestHandler(ICouponDbContext couponDbContext, IMapper mapper) : IRequestHandler<DeleteCouponRequest, Unit>
    {
        private readonly ICouponDbContext _repository = couponDbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
        {
            var coupon = await _repository.Coupons.FindAsync(new object[] { request.DeleteCoupon.Id }, cancellationToken);

            _repository.Coupons.Remove(coupon);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}