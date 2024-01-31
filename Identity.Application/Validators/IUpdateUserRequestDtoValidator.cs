using FluentValidation;
using Identity.Domain.DTOs.Identities;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Identity.Application.Validators
{
    public class IUpdateUserRequestDtoValidator : AbstractValidator<UpdateUserDto>
    {
        private readonly PineAppleIdentityDbContext _context;

        public IUpdateUserRequestDtoValidator(PineAppleIdentityDbContext context)
        {
            _context = context;
            RuleFor(key => key.Id).NotNull().NotEmpty();

            RuleFor(dto => dto.FirstName)
                .NotEmpty().NotNull()
                .MaximumLength(20).WithMessage("Строка не может превышать 20 символов");

            RuleFor(dto => dto.LastName)
                .NotEmpty().NotNull()
                .MaximumLength(20).WithMessage("Строка не может превышать 20 символов");

            RuleFor(dto => dto.UserName)
                .NotEmpty().NotNull()
                .When(dto => !string.IsNullOrEmpty(dto.UserName))
                .MaximumLength(50).WithMessage("Длина строки не должна превышать 50 символов")
                .MustAsync(BeUniqueUserName).WithMessage("Такое имя пользователя уже существует");

            RuleFor(dto => dto.EmailAddress)
                .NotEmpty().NotNull()
                .When(dto => !string.IsNullOrEmpty(dto.EmailAddress))
                .MaximumLength(50).WithMessage("Строка не может превышать 50 символов")
                .MustAsync(BeUniqueEmailAddress).WithMessage("Такой адрес электронной почты уже используется!")
                .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");
        }

        private async Task<bool> BeUniqueUserName(string userName, CancellationToken token)
        {
            var existsUserName = await _context.Users
                .FirstOrDefaultAsync(key => key.UserName == userName && key.Id != key.Id, token);

            if (existsUserName is not null)
                return false;

            return true;
        }

        private async Task<bool> BeUniqueEmailAddress(string emailAddress, CancellationToken token)
        {
            var existsEmailAddress = await _context.Users.FirstOrDefaultAsync(key => key.Email == emailAddress && key.Id != key.Id, token);

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