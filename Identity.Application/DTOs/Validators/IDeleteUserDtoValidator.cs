using FluentValidation;
using Identity.Application.DTOs.Identities;

namespace Identity.Application.DTOs.Validators
{
    public class IDeleteUserDtoValidator : AbstractValidator<DeleteUserDto>
    {
        public IDeleteUserDtoValidator()
        {
            RuleFor(key => key.Id).NotEmpty().NotNull();
        }
    }
}