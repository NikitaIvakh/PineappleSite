using Coupon.Domain.DTOs;
using Coupon.Infrastructure;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class CreateValidator : AbstractValidator<CreateCouponDto>
    {
        private readonly ApplicationDbContext _context;

        public CreateValidator(ApplicationDbContext context)
        {
            _context = context;
            Include(new ICouponRulesValidator(_context));
        }
    }
}