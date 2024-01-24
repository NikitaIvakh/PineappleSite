using FluentValidation;
using ShoppingCart.Application.DTOs.Cart;

namespace ShoppingCart.Application.DTOs.Validator
{
    public class ICartDetailsDtoValidator : AbstractValidator<CartDetailsDto>
    {
        public ICartDetailsDtoValidator()
        {
            RuleFor(key => key.Id).NotNull().NotEmpty();
        }
    }
}