using Coupon.Domain.DTOs;
using Coupon.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Validations
{
    public class CreateValidator : AbstractValidator<CreateCouponDto>
    {
        private readonly ApplicationDbContext _context;

        public CreateValidator(ApplicationDbContext context)
        {
            _context = context;
            
            RuleFor(key => key.CouponCode).NotEmpty().NotNull()
                .MaximumLength(20).WithMessage("Строка не должна превышать 20 символов")
                .MinimumLength(3).WithMessage("Строка должна превышать 3 символа")
                .MustAsync(BeUniqueCouponCode).WithMessage("Такой купон уже существует");

            RuleFor(key => key.DiscountAmount).NotEmpty().NotNull()
                .LessThanOrEqualTo(101).WithMessage("Сумма скидки не должна превышать 101 единицу")
                .GreaterThanOrEqualTo(2).WithMessage("Сумма скидки должна превышать 2 единицы")
                .MustAsync(DiscountAmountIsNotHigherThenProductAmount).WithMessage("Скидка не должна превышать стоимость продукта");

            RuleFor(key => key.MinAmount).NotEmpty().NotNull()
                .LessThanOrEqualTo(101).WithMessage("Цена товара должна быть ниже 101 единицы")
                .GreaterThanOrEqualTo(2).WithMessage("Сумма товара должна превышать 2 единицы");
        }

        private static Task<bool> DiscountAmountIsNotHigherThenProductAmount(CreateCouponDto coupon,
            double discountAmount, ValidationContext<CreateCouponDto> arg3, CancellationToken arg4)
        {
            return Task.FromResult(!(discountAmount > coupon.MinAmount));
        }

        private async Task<bool> BeUniqueCouponCode(string couponCode, CancellationToken token)
        {
            if (string.IsNullOrEmpty(couponCode) || !string.IsNullOrEmpty(couponCode))
            {
                return true;
            }

            var existsCouponCode = await _context.Coupons.FirstOrDefaultAsync(key => key.CouponCode == couponCode, token);
            return existsCouponCode is null;
        }
    }
}