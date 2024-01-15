using FluentValidation;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.DTOs.Identity.Validator
{
    public class IAuthRequestDroValidator : AbstractValidator<AuthRequestDto>
    {
        private readonly PineAppleIdentityDbContext _context;

        public IAuthRequestDroValidator(PineAppleIdentityDbContext context)
        {
            _context = context;

            RuleFor(key => key.Email).NotEmpty().NotNull().MaximumLength(50)
                .MustAsync(BeQniqueEmailAddress).WithMessage("Такой адрес электронной почты уже используется!");

            RuleFor(key => key.Password).NotEmpty().NotNull()
                .MaximumLength(50).WithMessage("Длина пароля не может превышать 50 символов");
        }

        private async Task<bool> BeQniqueEmailAddress(string emailAddress, CancellationToken token)
        {
            var existsEmail = await _context.Users.FirstOrDefaultAsync(key => key.Email == emailAddress, token);

            if (existsEmail is not null)
                return false;

            return true;
        }
    }
}