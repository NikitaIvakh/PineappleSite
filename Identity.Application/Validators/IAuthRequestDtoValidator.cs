using FluentValidation;
using Identity.Domain.DTOs.Authentications;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Validators
{
    public class IAuthRequestDtoValidator : AbstractValidator<AuthRequestDto>
    {
        private readonly PineAppleIdentityDbContext _context;

        public IAuthRequestDtoValidator(PineAppleIdentityDbContext context)
        {
            _context = context;

            RuleFor(key => key.Email).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(key => key.Password).NotEmpty().NotNull();
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