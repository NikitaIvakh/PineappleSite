using FluentValidation;
using ShoppingCart.Domain.DTOs;

namespace ShoppingCart.Application.Validators;

public sealed class DeleteByUserValidator : AbstractValidator<DeleteProductByUserDto>
{
    public DeleteByUserValidator()
    {
        RuleFor(key => key.ProductId)
            .NotNull().NotEmpty().WithMessage("Идентификатор продукта не может быть пустым");

        RuleFor(key => key.UserId)
            .NotNull().NotEmpty().WithMessage("Идентификатор пользователя не может быть пустым");
    }    
}