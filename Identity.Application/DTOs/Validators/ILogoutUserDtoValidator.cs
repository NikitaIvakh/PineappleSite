using FluentValidation;
using Identity.Application.DTOs.Identities;

namespace Identity.Application.DTOs.Validators
{
    public class ILogoutUserDtoValidator : AbstractValidator<LogoutUserDto>
    {
        public ILogoutUserDtoValidator()
        {
            RuleFor(key => key.UserId).NotNull().NotEmpty();
        }
    }
}