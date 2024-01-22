using FluentValidation;
using Product.Application.DTOs.Products;

namespace Product.Application.DTOs.Validator
{
    public class IDeleteProductsDtoValidator : AbstractValidator<DeleteProductsDto>
    {
        public IDeleteProductsDtoValidator()
        {
            RuleFor(key => key.ProductIds).NotEmpty().NotNull();
        }
    }
}