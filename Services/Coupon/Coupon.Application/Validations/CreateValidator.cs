using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class CreateValidator : AbstractValidator<CreateCouponDto>
    {
        public CreateValidator()
        {
            RuleFor(key => key.CouponCode).NotEmpty().NotNull()
                .MaximumLength(20).WithMessage("Строка не должна превышать 20 символов")
                .MinimumLength(3).WithMessage("Строка должна превышать 3 символа");

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
    }
}