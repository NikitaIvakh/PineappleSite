using FluentValidation;
using Order.Domain.DTOs;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Order.Application.Validators
{
    public class IOrderValidator : AbstractValidator<CartHeaderDto>
    {
        public IOrderValidator()
        {
            RuleFor(key => key.Name)
                .NotEmpty().NotNull().WithMessage("Поле с именем не может быть пустым")
                .MinimumLength(2).WithMessage("Имя должно быть более 2 символов")
                .MaximumLength(45).WithMessage("Имя не должно превышать 45 символов");

            RuleFor(key => key.Email)
                .NotEmpty().NotNull().WithMessage("Почтовый адрес не может быть пустым")
                .MinimumLength(2).WithMessage("Почтовый адрес должен быть более 2 символов")
                .MaximumLength(45).WithMessage("Почтовый адрес не должен превышать 45 символов")
                .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");

            RuleFor(key => key.PhoneNumber)
                .NotEmpty().NotNull().WithMessage("Поле с номером телефона не может быть пустым")
                .MinimumLength(2).WithMessage("Номер телефона должен быть более 7 символов")
                .MaximumLength(15).WithMessage("Номер телефона не должен превышать 12 символов")
                .MustAsync(BeValidMobilePhoneOrStandartPhone).WithMessage("Номер телефона не верного формата");
        }

        private Task<bool> BeValidEmailAddress(string emailAddress, CancellationToken token)
        {
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            bool isValid = Regex.IsMatch(emailAddress, emailPattern);
            return Task.FromResult(isValid);
        }

        private Task<bool> BeValidMobilePhoneOrStandartPhone(string phoneNumber, CancellationToken token)
        {
            string regexPattern = @"^375\d{9}$|^(\d{7})$";
            bool isValid = Regex.IsMatch(phoneNumber, regexPattern);

            return Task.FromResult(isValid);
        }
    }
}