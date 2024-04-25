using FluentValidation;
using Product.Domain.DTOs;

namespace Product.Application.Validations;

public sealed class DeleteProductsValidator : AbstractValidator<DeleteProductsDto>
{
    public DeleteProductsValidator()
    {
        RuleFor(key => key.ProductIds).NotEmpty().NotNull()
            .WithMessage("Идентификаторы продуктов не могут быть пустыми");
    }
}