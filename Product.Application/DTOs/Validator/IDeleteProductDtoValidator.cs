using FluentValidation;
using Product.Application.DTOs.Products;

namespace Product.Application.DTOs.Validator
{
    public class IDeleteProductDtoValidator : AbstractValidator<DeleteProductDto>
    {
        public IDeleteProductDtoValidator()
        {
            RuleFor(key => key.Id).NotEmpty().NotNull();
        }
    }
}