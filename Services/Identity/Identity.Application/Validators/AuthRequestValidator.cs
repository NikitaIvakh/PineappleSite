using FluentValidation;
using Identity.Domain.DTOs.Authentications;
using System.Text.RegularExpressions;

namespace Identity.Application.Validators;

public sealed partial class AuthRequestValidator : AbstractValidator<AuthRequestDto>
{
    public AuthRequestValidator()
    {
        RuleFor(key => key.EmailAddress)
            .NotNull().NotEmpty().WithMessage("Адрес электронной почты не может быть пустым")
            .When(dto => !string.IsNullOrEmpty(dto.EmailAddress))
            .MinimumLength(2).WithMessage("Адрес электронной почты должен быть более 2 символов")
            .MaximumLength(50).WithMessage("Адрес электронной почты не может превышать 50 символов")
            .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");

        RuleFor(key => key.Password)
            .NotNull().NotEmpty().WithMessage("Пароль не может быть пустым")
            .MinimumLength(5).WithMessage("Пароль должен быть более 5 символов")
            .MaximumLength(50).WithMessage("Пароль не может превышать 50 символов");
    }

    private static Task<bool> BeValidEmailAddress(string emailAddress, CancellationToken arg2)
    {
        var isValid = MyRegex().IsMatch(emailAddress);
        return Task.FromResult(isValid);
    }

    // private static Task<bool> BeValidEmailAddress(string emailAddress, CancellationToken token)
    // {
    //     var isValid = MyRegex().IsMatch(emailAddress);
    //     return Task.FromResult(isValid);
    // }

    [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
    private static partial Regex MyRegex();
}