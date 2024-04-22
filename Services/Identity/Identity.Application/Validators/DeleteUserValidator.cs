using FluentValidation;
using Identity.Domain.DTOs.Identities;

namespace Identity.Application.Validators;

public sealed class DeleteUserValidator : AbstractValidator<DeleteUserDto>
{
    public DeleteUserValidator()
    {
        RuleFor(key => key.UserId)
            .NotEmpty().NotNull().WithMessage("Идентификатор не может быть пустым");
    }
}