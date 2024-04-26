using FluentValidation;
using ShoppingCart.Domain.DTOs;

namespace ShoppingCart.Application.Validators;

public sealed class DeleteValidator : AbstractValidator<DeleteProductDto>
{
    public DeleteValidator()
    {
        RuleFor(key => key.ProductId)
            .NotNull().NotEmpty().WithMessage("Идентификатор продукта не может быть пустым");
    }
}