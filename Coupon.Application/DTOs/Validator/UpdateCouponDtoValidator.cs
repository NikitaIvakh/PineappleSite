using Coupon.Application.Interfaces;
using FluentValidation;

namespace Coupon.Application.DTOs.Validator
{
    public class UpdateCouponDtoValidator : AbstractValidator<UpdateCouponDto>
    {
        private readonly ICouponDbContext _context;

        public UpdateCouponDtoValidator(ICouponDbContext context)
        {
            _context = context;
            RuleFor(key => key.CouponId).NotNull().NotEmpty();
            Include(key => new ICouponDtoValidator(_context));
        }
    }
}