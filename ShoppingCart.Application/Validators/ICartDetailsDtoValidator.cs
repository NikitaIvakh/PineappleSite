using FluentValidation;
using ShoppingCart.Domain.DTOs;

namespace ShoppingCart.Application.Validators
{
    public class ICartDetailsDtoValidator : AbstractValidator<CartDetailsDto>
    {
        public ICartDetailsDtoValidator()
        {
            RuleFor(key => key.Id).NotNull().NotEmpty();
        }
    }
}