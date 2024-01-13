using Coupon.Application.Interfaces;
using FluentValidation;

namespace Coupon.Application.DTOs.Validator
{
    public class CreateCouponDtoValidfator : AbstractValidator<CreateCouponDto>
    {
        private readonly ICouponDbContext _context;

        public CreateCouponDtoValidfator(ICouponDbContext context)
        {
            _context = context;
            Include(key => new ICouponDtoValidfator(_context));
        }
    }
}