using FluentValidation;

namespace Coupon.Application.DTOs.Validator
{
    public class DeleteCouponListDtoValidator : AbstractValidator<DeleteCouponListDto>
    {
        public DeleteCouponListDtoValidator()
        {
            RuleFor(dto => dto.CouponIds).NotEmpty().WithMessage("Список идентификаторов купонов не может быть пустым.");
        }
    }
}