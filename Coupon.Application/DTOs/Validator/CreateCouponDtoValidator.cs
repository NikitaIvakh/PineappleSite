using Coupon.Application.Interfaces;
using FluentValidation;

namespace Coupon.Application.DTOs.Validator
{
    public class CreateCouponDtoValidator : AbstractValidator<CreateCouponDto>
    {
        private readonly ICouponDbContext _context;

        public CreateCouponDtoValidator(ICouponDbContext context)
        {
            _context = context;
            Include(key => new ICouponDtoValidator(_context));
        }
    }
}