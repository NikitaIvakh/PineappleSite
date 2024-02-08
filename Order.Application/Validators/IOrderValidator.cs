using FluentValidation;
using Order.Domain.DTOs;
using System.Text.RegularExpressions;

namespace Order.Application.Validators
{
    public class IOrderValidator : AbstractValidator<OrderHeaderDto>
    {
        public IOrderValidator()
        {
            RuleFor(key => key.Name)
                .NotEmpty().NotNull().WithMessage("Поле с именем не может быть пустым")
                .MinimumLength(2).WithMessage("Имя должно быть более 2 символов")
                .MaximumLength(15).WithMessage("Имя не должно превышать 15 символов");

            RuleFor(key => key.Email)
                .NotEmpty().NotNull().WithMessage("Почтовый адрес не может быть пустым")
                .MinimumLength(2).WithMessage("Почтовый адрес должен быть более 2 символов")
                .MaximumLength(15).WithMessage("Почтовый адрес не должен превышать 15 символов");

            RuleFor(key => key.PhoneNumber)
                     .NotEmpty().NotNull().WithMessage("Поле с номером телефона не может быть пустым")
                     .MinimumLength(2).WithMessage("Номер телефона должен быть более 7 символов")
                     .MaximumLength(15).WithMessage("Номер телефона не должен превышать 12 символов")
                     .MustAsync(BeValidMobilePhoneOrStandartPhone).WithMessage("Номер телефона не верного формата");
        }

        private Task<bool> BeValidMobilePhoneOrStandartPhone(string phoneNumber, CancellationToken token)
        {
            string regexPattern = @"^375\d{9}$|^(\d{7})$";
            bool isValid = Regex.IsMatch(phoneNumber, regexPattern);

            return Task.FromResult(isValid);
        }
    }
}