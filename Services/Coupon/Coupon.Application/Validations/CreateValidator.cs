using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations;

public sealed class CreateValidator : AbstractValidator<CreateCouponDto>
{
    public CreateValidator()
    {
        RuleFor(key => key.CouponCode).NotEmpty().NotNull()
            .MaximumLength(20).WithMessage("Код купона не должен превышать 20 символов")
            .MinimumLength(3).WithMessage("Код купона должен превышать 3 символа");

        RuleFor(key => key.DiscountAmount).NotEmpty().NotNull()
            .LessThanOrEqualTo(101).WithMessage("Сумма скидки купона не должна превышать 101 единицу")
            .GreaterThanOrEqualTo(2).WithMessage("Сумма скидки купона должна превышать 2 единицы")
            .MustAsync(DiscountAmountIsNotHigherThenProductAmount)
            .WithMessage("Сумма скидки не должна превышать стоимость продукта");

        RuleFor(key => key.MinAmount).NotEmpty().NotNull()
            .LessThanOrEqualTo(101).WithMessage("Стоимость товара должна быть ниже 101 единицы")
            .GreaterThanOrEqualTo(2).WithMessage("Стоимость товара должна превышать 2 единицы");
    }

    private static Task<bool> DiscountAmountIsNotHigherThenProductAmount(CreateCouponDto coupon,
        double discountAmount, ValidationContext<CreateCouponDto> arg3, CancellationToken arg4)
    {
        return Task.FromResult(coupon.MinAmount > discountAmount);
    }
}