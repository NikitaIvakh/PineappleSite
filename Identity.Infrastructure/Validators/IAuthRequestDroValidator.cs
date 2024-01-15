using FluentValidation;
using Identity.Core.Entities.Identities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Validators
{
    public class IAuthRequestDroValidator : AbstractValidator<AuthRequest>
    {
        private readonly PineAppleIdentityDbContext _context;

        public IAuthRequestDroValidator(PineAppleIdentityDbContext context)
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