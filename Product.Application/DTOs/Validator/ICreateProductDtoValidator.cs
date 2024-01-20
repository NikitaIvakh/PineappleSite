using FluentValidation;
using Product.Application.DTOs.Products;

namespace Product.Application.DTOs.Validator
{
    public class ICreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public ICreateProductDtoValidator()
        {
            Include(new IProductDtoValidator());
        }
    }
}