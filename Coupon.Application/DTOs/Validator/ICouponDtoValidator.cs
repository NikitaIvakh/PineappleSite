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
                .LessThanOrEqualTo(100).WithMessage("Сумма скидки не должна превышать 100 единиц")
                .GreaterThanOrEqualTo(2).WithMessage("Сумма скидки должна превышать 2 единицы")
                .MustAsync(DiscountAmountIsNotHigherThenProductAmount).WithMessage("Скидка не должна превышать стоимость продукта");

            RuleFor(key => key.MinAmount).NotEmpty().NotNull()
                .LessThanOrEqualTo(100).WithMessage("Цена товара должна быть ниже 100 единиц")
                .GreaterThanOrEqualTo(2).WithMessage("Сумма товара должна превышать 2 единицы");
        }

        private async Task<bool> BeQniqueCouponCode(string couponCode, CancellationToken token)
        {
            var couponCodeIsExists = await _context.Coupons.FirstOrDefaultAsync(key => key.CouponCode == couponCode, token);

            if (couponCodeIsExists is not null)
                return false;

            return true;
        }

        private async Task<bool> DiscountAmountIsNotHigherThenProductAmount(ICouponDto couponDto, double discountAmount, CancellationToken token)
        {
            if (discountAmount > couponDto.MinAmount)
            {
                return false;
            }

            return true;
        }
    }
}