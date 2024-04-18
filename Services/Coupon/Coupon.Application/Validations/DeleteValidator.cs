using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations;

public class DeleteValidator : AbstractValidator<DeleteCouponDto>
{
    public DeleteValidator()
    {
        RuleFor(key => key.CouponId)
            .NotEmpty().NotNull().WithMessage("Идентификатор купона не должен быть пустым");
    }
}