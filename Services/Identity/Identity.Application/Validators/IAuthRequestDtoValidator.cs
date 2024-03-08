using FluentValidation;
using Identity.Domain.DTOs.Authentications;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace Identity.Application.Validators
{
    public class IAuthRequestDtoValidator : AbstractValidator<AuthRequestDto>
    {
        public IAuthRequestDtoValidator()
        {
            RuleFor(key => key.Email)
                .NotNull().NotEmpty().WithMessage("Адрес электронной почты не может быть пустым")
                .When(dto => !string.IsNullOrEmpty(dto.Email))
                .MinimumLength(2).WithMessage("Адрес электронной почты должен быть более 2 символов")
                .MaximumLength(50).WithMessage("Адрес электронной почты не может превышать 50 символов")
                .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");

            RuleFor(key => key.Password)
                .NotNull().NotEmpty().WithMessage("Пароль не может быть пустым")
                .MinimumLength(5).WithMessage("Пароль должен быть более 5 символов")
                .MaximumLength(50).WithMessage("Пароль не может превышать 50 символов");
        }

        private Task<bool> BeValidEmailAddress(string emailAddress, CancellationToken token)
        {
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            bool isValid = Regex.IsMatch(emailAddress, emailPattern);
            return Task.FromResult(isValid);
        }
    }
}