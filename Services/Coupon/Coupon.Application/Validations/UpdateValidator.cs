using Coupon.Domain.DTOs;
using Coupon.Infrastructure;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class UpdateValidator : AbstractValidator<UpdateCouponDto>
    {
        private readonly ApplicationDbContext _context;

        public UpdateValidator(ApplicationDbContext context)
        {
            _context = context;
            Include(new ICouponRulesValidator(_context));
        }
    }
}