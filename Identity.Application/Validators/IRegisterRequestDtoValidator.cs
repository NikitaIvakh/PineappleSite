using FluentValidation;
using Identity.Domain.DTOs.Authentications;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Identity.Application.Validators
{
    public class IRegisterRequestDtoValidator : AbstractValidator<IRegisterRequestDto>
    {
        private readonly PineAppleIdentityDbContext _context;

        public IRegisterRequestDtoValidator(PineAppleIdentityDbContext context)
        {
            _context = context;

            RuleFor(key => key.FirstName)
                .NotNull().NotEmpty().WithMessage("Имя не может быть пустым")
                .MinimumLength(2).WithMessage("Имя должно быть более 2 символов")
                .MaximumLength(20).WithMessage("Имя не может превышать 20 символов");

            RuleFor(key => key.LastName)
                .NotNull().NotEmpty().WithMessage("Фамилия не может быть пустой")
                .MinimumLength(2).WithMessage("Фамилия должна быть более 2 символов")
                .MaximumLength(20).WithMessage("Фамилия не может превышать 20 символов");

            RuleFor(key => key.UserName)
                .NotNull().NotEmpty().WithMessage("Имя пользователя не может быть пустым")
                .When(key => !string.IsNullOrEmpty(key.UserName))
                .MinimumLength(5).WithMessage("Длина строки имени пользователя должна быть более 5 символов")
                .MaximumLength(50).WithMessage("Длина строки имени пользователя не должна превышать 50 символов")
                .MustAsync(BeUniqueUserName).WithMessage("Такое имя пользователя уже существует");

            RuleFor(key => key.EmailAddress)
                .NotNull().NotEmpty().WithMessage("Адрес электронной почты не может быть пустым")
                .When(key => !string.IsNullOrEmpty(key.EmailAddress))
                .MinimumLength(2).WithMessage("Адрес электронной почты должн быть более 2 символов")
                .MaximumLength(50).WithMessage("Адрес электронной почты не может превышать 50 символов")
                .MustAsync(BeUniqueEmailAddress).WithMessage("Такой адрес электронной почты уже используется")
                .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");

            RuleFor(key => key.Password)
                .NotNull().NotEmpty().WithMessage("Пароль не может быть пустым")
                .MinimumLength(5).WithMessage("Пароль должн быть более 5 символов")
                .MaximumLength(50).WithMessage("Пароль не может превышать 50 символов");
        }

        private async Task<bool> BeUniqueUserName(string userName, CancellationToken token)
        {
            var existsUserName = await _context.Users.FirstOrDefaultAsync(key => key.UserName == userName, token);

            if (existsUserName is not null)
                return false;

            return true;
        }

        private async Task<bool> BeUniqueEmailAddress(string emailAddress, CancellationToken token)
        {
            var existsEmailAddress = await _context.Users.FirstOrDefaultAsync(key => key.Email == emailAddress, token);

            if (existsEmailAddress is not null)
                return false;

            return true;
        }

        private Task<bool> BeValidEmailAddress(string emailAddress, CancellationToken token)
        {
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            bool isValid = Regex.IsMatch(emailAddress, emailPattern);
            return Task.FromResult(isValid);
        }
    }
}