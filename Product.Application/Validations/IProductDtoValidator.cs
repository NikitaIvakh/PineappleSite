using FluentValidation;
using Product.Domain.DTOs;

namespace Product.Application.DTOs.Validator
{
    public class IProductDtoValidator : AbstractValidator<IProductDto>
    {
        public IProductDtoValidator()
        {
            RuleFor(key => key.Name).NotEmpty().NotNull()
                .MaximumLength(30).WithMessage("Строка не должна превышать 30 символов.")
                .MinimumLength(3).WithMessage("Строка должна быть более 3 символов.");

            RuleFor(key => key.Description).NotEmpty().NotNull()
                .MaximumLength(500).WithMessage("Строка не должна превышать 500 символов.")
                .MinimumLength(10).WithMessage("Строка должна быть более 10 символов.");

            RuleFor(key => key.ProductCategory)
                .NotEmpty().WithMessage("Категория продукта не может быть пустой.")
                .NotNull().WithMessage("Категория продукта должна быть.")
                .IsInEnum().WithMessage("Неверная категория продукта.");

            RuleFor(key => key.Price).NotEmpty().NotNull()
               .LessThanOrEqualTo(1000).WithMessage("Стоимость продукта не должна превышать 1000.")
               .GreaterThanOrEqualTo(5).WithMessage("Стоимость товара доложна быть более 5.");
        }
    }
}