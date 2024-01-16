using FluentValidation;
using Identity.Application.DTOs.Authentications;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Identity.Application.DTOs.Validators
{
    public class IRegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        private readonly PineAppleIdentityDbContext _context;

        public IRegisterRequestDtoValidator(PineAppleIdentityDbContext context)
        {
            _context = context;

            RuleFor(key => key.FirstName).NotEmpty().NotNull()
                 .MaximumLength(20).WithMessage("Строка не может превышать 20 символов");

            RuleFor(key => key.LastName).NotEmpty().NotNull()
              .MaximumLength(20).WithMessage("Строка не может превышать 20 символов");

            RuleFor(key => key.EmailAddress).NotEmpty().NotNull()
                .MaximumLength(50).WithMessage("Строка не может превышать 50 символов")
                .MustAsync(BeUniqueEmailAddress).WithMessage("Такой адрес электронной почты уже используется!")
                .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");

            RuleFor(key => key.UserName).NotEmpty().NotNull()
                .MaximumLength(50).WithMessage("Длина строки не должна превышать 50 символов")
                .MustAsync(BeUniqueUserName).WithMessage("Такое имя пользователя уже существует");

            RuleFor(key => key.Password).NotEmpty().NotNull()
                .MaximumLength(50).WithMessage("Строка не может превышать 50 символов");
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