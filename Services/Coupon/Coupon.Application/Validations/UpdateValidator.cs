using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class UpdateValidator : AbstractValidator<UpdateCouponDto>
    {
        public UpdateValidator()
        {
            RuleFor(key => key.CouponId)
                .NotEmpty().NotNull().WithMessage("Идентификатор купона не может быть пустым");
            
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

        private static Task<bool> BeUniqueCouponCode(UpdateCouponDto coupon, string couponCode, ValidationContext<UpdateCouponDto> arg3, CancellationToken arg4)
        {
            if (string.IsNullOrEmpty(couponCode) || !string.IsNullOrEmpty(couponCode))
            {
                return Task.FromResult(true);
            }

            var existsCouponCode = coupon.CouponCode == couponCode;
            return Task.FromResult(existsCouponCode);
        }

        private static Task<bool> DiscountAmountIsNotHigherThenProductAmount(UpdateCouponDto coupon,
            double discountAmount, ValidationContext<UpdateCouponDto> arg3, CancellationToken arg4)
        {
            return Task.FromResult(!(discountAmount > coupon.MinAmount));
        }
    }
}