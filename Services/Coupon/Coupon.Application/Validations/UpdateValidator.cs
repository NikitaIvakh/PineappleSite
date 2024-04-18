using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations;

public class UpdateValidator : AbstractValidator<UpdateCouponDto>
{
    public UpdateValidator()
    {
        RuleFor(key => key.CouponId)
            .NotEmpty().NotNull().WithMessage("Идентификатор купона не должен быть пустым");

        RuleFor(key => key.CouponCode).NotEmpty().NotNull()
            .MaximumLength(20).WithMessage("Код купона не должен превышать 20 символов")
            .MinimumLength(3).WithMessage("Код купона должен превышать 3 символа")
            .MustAsync(BeUniqueCouponCode).WithMessage("Такой купон уже существует");

        RuleFor(key => key.DiscountAmount).NotEmpty().NotNull()
            .LessThanOrEqualTo(101).WithMessage("Сумма скидки купона не должна превышать 101 единицу")
            .GreaterThanOrEqualTo(2).WithMessage("Сумма скидки купона должна превышать 2 единицы")
            .MustAsync(DiscountAmountIsNotHigherThenProductAmount)
            .WithMessage("Сумма скидки не должна превышать стоимость продукта");

        RuleFor(key => key.MinAmount).NotEmpty().NotNull()
            .LessThanOrEqualTo(101).WithMessage("Стоимость товара должна быть ниже 101 единицы")
            .GreaterThanOrEqualTo(2).WithMessage("Стоимость товара должна превышать 2 единицы");
    }

    private static Task<bool> BeUniqueCouponCode(UpdateCouponDto coupon, string couponCode,
        ValidationContext<UpdateCouponDto> arg3, CancellationToken arg4)
    {
        return Task.FromResult(string.IsNullOrEmpty(couponCode) || !string.IsNullOrEmpty(couponCode));
    }

    private static Task<bool> DiscountAmountIsNotHigherThenProductAmount(UpdateCouponDto coupon,
        double discountAmount, ValidationContext<UpdateCouponDto> arg3, CancellationToken arg4)
    {
        return Task.FromResult(coupon.MinAmount > discountAmount);
    }
}