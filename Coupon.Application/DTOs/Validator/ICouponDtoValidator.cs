using Coupon.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.DTOs.Validator
{
    public class ICouponDtoValidator : AbstractValidator<ICouponDto>
    {
        private readonly ICouponDbContext _context;

        public ICouponDtoValidator(ICouponDbContext context)
        {
            _context = context;

            RuleFor(key => key.CouponCode).NotEmpty().NotNull().MustAsync(BeQniqueCouponCode).WithMessage("Такой купон уже существует")
                .MaximumLength(20).WithMessage("Строка не должна превышать 20 символов")
                .MinimumLength(3).WithMessage("Строка должна превышать 3 символа");

            RuleFor(key => key.DiscountAmount).NotEmpty().NotNull()
                .LessThanOrEqualTo(2).WithMessage("Сумма скидки не доложна быть ниже 2 единиц")
                .GreaterThanOrEqualTo(100).WithMessage("Сумма скидки не должна превышать 100 единиц");

            RuleFor(key => key.MinAmount).NotEmpty().NotNull()
                .LessThanOrEqualTo(7).WithMessage("Цена товара не должна быть ниже 7 единиц")
                .GreaterThanOrEqualTo(100).WithMessage("Сумма товара не должна превышать 100 единиц");
        }

        private async Task<bool> BeQniqueCouponCode(string couponCode, CancellationToken token)
        {
            var couponCodeIsExists = await _context.Coupons.FirstOrDefaultAsync(key => key.CouponCode == couponCode, token);

            if (couponCodeIsExists is not null)
                return false;

            return true;
        }
    }
}