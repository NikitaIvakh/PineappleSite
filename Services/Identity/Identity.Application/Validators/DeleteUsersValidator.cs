using FluentValidation;
using Identity.Domain.DTOs.Identities;

namespace Identity.Application.Validators;

public sealed class DeleteUsersValidator : AbstractValidator<DeleteUsersDto>
{
    public DeleteUsersValidator()
    {
        RuleFor(key => key.UserIds)
            .NotNull().NotEmpty().WithMessage("Идентификаторы не могут быть пустыми");
    }
}