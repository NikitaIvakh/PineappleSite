using FluentValidation;

namespace Coupon.Application.DTOs.Validator
{
    public class DeleteCouponDtoValidator : AbstractValidator<DeleteCouponDto>
    {
        public DeleteCouponDtoValidator()
        {
            RuleFor(key => key.Id).NotEmpty().NotNull();
        }
    }
}