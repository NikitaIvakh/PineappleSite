using FluentValidation;
using Identity.Domain.DTOs.Identities;

namespace Identity.Application.Validators
{
    public class ILogoutUserDtoValidator : AbstractValidator<LogoutUserDto>
    {
        public ILogoutUserDtoValidator()
        {
            RuleFor(key => key.UserId).NotNull().NotEmpty();
        }
    }
}