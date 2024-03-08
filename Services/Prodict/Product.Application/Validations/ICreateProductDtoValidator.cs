using FluentValidation;
using Product.Domain.DTOs;

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