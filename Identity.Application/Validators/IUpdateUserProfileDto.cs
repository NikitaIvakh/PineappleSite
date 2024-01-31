using FluentValidation;
using Identity.Domain.DTOs.Identities;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Identity.Application.Validators
{
    public class IUpdateUserProfileDto : AbstractValidator<UpdateUserProfileDto>
    {
        private readonly PineAppleIdentityDbContext _context;

        public IUpdateUserProfileDto(PineAppleIdentityDbContext context)
        {
            _context = context;
            RuleFor(key => key.Id).NotNull().NotEmpty();

            RuleFor(dto => dto.FirstName)
                .MaximumLength(20).WithMessage("Строка не может превышать 20 символов");

            RuleFor(dto => dto.LastName).MaximumLength(20).WithMessage("Строка не может превышать 20 символов");

            RuleFor(dto => dto.UserName)
                .MaximumLength(50).WithMessage("Длина строки не должна превышать 50 символов")
                .MustAsync(BeUniqueUserName).WithMessage("Такое имя пользователя уже существует");

            RuleFor(dto => dto.EmailAddress)
                .MaximumLength(50).WithMessage("Строка не может превышать 50 символов")
                .MustAsync(BeUniqueEmailAddress).WithMessage("Такой адрес электронной почты уже используется!")
                .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");

            RuleFor(key => key.Description)
            .MinimumLength(10).WithMessage("Длина строки должна быть более 10 символов")
            .MaximumLength(300).WithMessage("Длина строки не может превышать 300 символов");

            RuleFor(key => key.Age)
            .GreaterThanOrEqualTo(18).WithMessage("Возраст должен быть больше 18")
            .LessThanOrEqualTo(120).WithMessage("Возраст не должен превышать 120");

            RuleFor(key => key.Password)
                .MinimumLength(3).WithMessage("Длина пароля должна быть более 3 символов")
                .MaximumLength(30).WithMessage("Пароль не может превышать 30 символов");
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