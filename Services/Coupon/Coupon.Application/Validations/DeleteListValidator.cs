using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class DeleteListValidator : AbstractValidator<DeleteCouponListDto>
    {
        public DeleteListValidator()
        {
            RuleFor(dto => dto.CouponIds).NotEmpty().WithMessage("Список идентификаторов купонов не может быть пустым.");
        }
    }
}