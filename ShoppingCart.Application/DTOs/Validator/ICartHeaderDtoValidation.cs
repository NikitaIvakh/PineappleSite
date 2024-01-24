using FluentValidation;
using ShoppingCart.Application.DTOs.Cart;

namespace ShoppingCart.Application.DTOs.Validator
{
    public class ICartHeaderDtoValidation : AbstractValidator<CartHeaderDto>
    {
        public ICartHeaderDtoValidation()
        {
            RuleFor(key => key.Id).NotEmpty().NotNull();
            RuleFor(key => key.UserId).NotEmpty().NotNull();
            RuleFor(key => key.CouponCode).MaximumLength(20).WithMessage("Длина строки не может превышать 20 символов");
            RuleFor(key => key.Discount).GreaterThanOrEqualTo(1).WithMessage("Сумма скидки не может быть меньше 1");
        }
    }
}