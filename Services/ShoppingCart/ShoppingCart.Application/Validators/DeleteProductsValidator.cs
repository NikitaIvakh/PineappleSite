using FluentValidation;
using ShoppingCart.Domain.DTOs;

namespace ShoppingCart.Application.Validators;

public sealed class DeleteProductsValidator : AbstractValidator<DeleteProductsDto>
{
    public DeleteProductsValidator()
    {
        RuleFor(key => key.ProductIds)
            .NotNull().NotEmpty().WithMessage("Идентификаторы продуктов не могут быть пустыми");
    }
}