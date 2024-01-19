using FluentValidation;
using Identity.Application.DTOs.Identities;

namespace Identity.Application.DTOs.Validators
{
    public class IDeleteUserListDtoValidator : AbstractValidator<DeleteUserListDto>
    {
        public IDeleteUserListDtoValidator()
        {
            RuleFor(key => key.UserIds).NotNull().NotEmpty().NotEqual([]);
        }
    }
}