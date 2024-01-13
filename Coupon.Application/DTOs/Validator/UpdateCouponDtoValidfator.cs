using Coupon.Application.Interfaces;
using FluentValidation;

namespace Coupon.Application.DTOs.Validator
{
    public class UpdateCouponDtoValidfator : AbstractValidator<UpdateCouponDto>
    {
        private readonly ICouponDbContext _context;

        public UpdateCouponDtoValidfator(ICouponDbContext context)
        {
            _context = context;
            RuleFor(key => key.CouponId).NotNull().NotEmpty();
            Include(key => new ICouponDtoValidfator(_context));
        }
    }
}