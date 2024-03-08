using FluentValidation;
using Product.Domain.DTOs;

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