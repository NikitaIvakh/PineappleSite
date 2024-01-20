using FluentValidation;
using Product.Application.DTOs.Products;

namespace Product.Application.DTOs.Validator
{
    public class IUpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public IUpdateProductDtoValidator()
        {
            Include(new IProductDtoValidator());
        }
    }
}