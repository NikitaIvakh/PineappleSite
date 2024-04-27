using Favourite.Domain.DTOs;
using FluentValidation;

namespace Favourite.Application.Validators;

public sealed class DeleteByUserValidator : AbstractValidator<DeleteFavouriteProductByUserDto>
{
    public DeleteByUserValidator()
    {
        RuleFor(key => key.ProductId)
            .NotEmpty().NotNull().WithMessage("Идентификатор продукта не может быть пустым");

        RuleFor(key => key.UserId)
            .NotNull().NotEmpty().WithMessage("Идентификатор пользователя не может быть пустым");
    }
}