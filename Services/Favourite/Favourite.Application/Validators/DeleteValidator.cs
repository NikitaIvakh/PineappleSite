using Favourite.Domain.DTOs;
using FluentValidation;

namespace Favourite.Application.Validators;

public sealed class DeleteValidator : AbstractValidator<DeleteFavouriteProductDto>
{
    public DeleteValidator()
    {
        RuleFor(key => key.Id)
            .NotNull().NotEmpty().WithMessage("Идектификатор продукта не может быть пустым");
    }
}