using FluentValidation;
using Identity.Application.DTOs.Identities;
using Identity.Infrastructure;

namespace Identity.Application.DTOs.Validators
{
    public class IUpdateUserRequestDtoValidator : AbstractValidator<UpdateUserDto>
    {
        private readonly PineAppleIdentityDbContext _context;

        public IUpdateUserRequestDtoValidator(PineAppleIdentityDbContext context)
        {
            _context = context;
            RuleFor(key => key.Id).NotNull().NotEmpty();
            Include(new IRegisterRequestDtoValidator(_context));
        }
    }
}