using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class ICouponRulesValidator : AbstractValidator<ICouponDto>
    {
        public ICouponRulesValidator()
        {
            RuleFor(key => key.CouponCode).NotEmpty().NotNull()
                .MaximumLength(20).WithMessage("Строка не должна превышать 20 символов")
                .MinimumLength(3).WithMessage("Строка должна превышать 3 символа");

            RuleFor(key => key.DiscountAmount).NotEmpty().NotNull()
                .LessThanOrEqualTo(101).WithMessage("Сумма скидки не должна превышать 101 единиц")
                .GreaterThanOrEqualTo(2).WithMessage("Сумма скидки должна превышать 2 единицы")
                .MustAsync(DiscountAmountIsNotHigherThenProductAmount).WithMessage("Скидка не должна превышать стоимость продукта");

            RuleFor(key => key.MinAmount).NotEmpty().NotNull()
                .LessThanOrEqualTo(101).WithMessage("Цена товара должна быть ниже 101 единиц")
                .GreaterThanOrEqualTo(2).WithMessage("Сумма товара должна превышать 2 единицы");
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