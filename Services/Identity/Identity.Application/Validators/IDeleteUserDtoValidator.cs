using FluentValidation;
using Identity.Domain.DTOs.Identities;

namespace Identity.Application.Validators
{
    public class IDeleteUserDtoValidator : AbstractValidator<DeleteUserDto>
    {
        public IDeleteUserDtoValidator()
        {
            RuleFor(key => key.Id).NotEmpty().NotNull();
        }
    }
}