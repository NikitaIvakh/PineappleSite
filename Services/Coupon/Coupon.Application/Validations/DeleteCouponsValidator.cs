using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class DeleteCouponsValidator : AbstractValidator<DeleteCouponsDto>
    {
        public DeleteCouponsValidator()
        {
            RuleFor(dto => dto.CouponIds)
                .NotEmpty().NotNull().WithMessage("Список идентификаторов купонов не может быть пустым.");
        }
    }
}