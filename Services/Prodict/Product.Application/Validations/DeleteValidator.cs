using FluentValidation;
using Product.Domain.DTOs;

namespace Product.Application.Validations;

public sealed class DeleteValidator : AbstractValidator<DeleteProductDto>
{
    public DeleteValidator()
    {
        RuleFor(key => key.Id).NotEmpty().NotNull().WithMessage("Идентификатор продукта не может быть пустым");
    }
}