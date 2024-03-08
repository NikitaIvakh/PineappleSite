using FluentValidation;
using Product.Domain.DTOs;

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