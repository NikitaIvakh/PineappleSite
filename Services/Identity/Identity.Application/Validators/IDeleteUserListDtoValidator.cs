using FluentValidation;
using Identity.Domain.DTOs.Identities;

namespace Identity.Application.Validators
{
    public class IDeleteUserListDtoValidator : AbstractValidator<DeleteUserListDto>
    {
        public IDeleteUserListDtoValidator()
        {
            RuleFor(key => key.UserIds).NotNull().NotEmpty().NotEqual([]);
        }
    }
}