using Favourite.Domain.DTOs;
using FluentValidation;

namespace Favourite.Application.Validators;

public class DeleteProductsValidator  : AbstractValidator<DeleteFavouriteProductsDto>
{
    public DeleteProductsValidator()
    {
        RuleFor(key => key.ProductIds)
            .NotEmpty().NotNull().WithMessage("дентификаторы продуктов не могут быть пустыми");
    }
}