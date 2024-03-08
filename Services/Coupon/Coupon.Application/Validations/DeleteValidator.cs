using Coupon.Domain.DTOs;
using Coupon.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Validations
{
    public class DeleteValidator : AbstractValidator<DeleteCouponDto>
    {
        private readonly ApplicationDbContext _context;

        public DeleteValidator(ApplicationDbContext context)
        {
            _context = context;
            RuleFor(key => key.Id).NotEmpty().NotNull()
                .MustAsync(BeTrueId).WithMessage("Такого id не существует");
        }

        private async Task<bool> BeTrueId(int couponId, CancellationToken token)
        {
            bool exists = await _context.Coupons.AnyAsync(coupon => coupon.CouponId == couponId, token);
            return exists;
        }
    }
}