using FluentValidation;
using Identity.Domain.DTOs.Identities;
using System.Text.RegularExpressions;

namespace Identity.Application.Validators;

public sealed partial class CreateUserValidation : AbstractValidator<CreateUserDto>
{
    public CreateUserValidation()
    {
        RuleFor(key => key.FirstName)
            .NotNull().NotEmpty().WithMessage("Имя не может быть пустым")
            .MinimumLength(2).WithMessage("Имя должно быть более 2 символов")
            .MaximumLength(20).WithMessage("Имя не может превышать 20 символов");

        RuleFor(key => key.LastName)
            .NotNull().NotEmpty().WithMessage("Фамилия не может быть пустой")
            .MinimumLength(2).WithMessage("Фамилия должна быть более 2 символов")
            .MaximumLength(20).WithMessage("Фамилия не может превышать 20 символов");

        RuleFor(key => key.UserName)
            .NotNull().NotEmpty().WithMessage("Имя пользователя не может быть пустым")
            .When(key => !string.IsNullOrEmpty(key.UserName))
            .MinimumLength(5).WithMessage("Длина строки имени пользователя должна быть более 5 символов")
            .MaximumLength(50).WithMessage("Длина строки имени пользователя не должна превышать 50 символов");

        RuleFor(key => key.EmailAddress)
            .NotNull().NotEmpty().WithMessage("Адрес электронной почты не может быть пустым")
            .When(key => !string.IsNullOrEmpty(key.EmailAddress))
            .MinimumLength(2).WithMessage("Адрес электронной почты должен быть более 2 символов")
            .MaximumLength(50).WithMessage("Адрес электронной почты не может превышать 50 символов")
            .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");

        RuleFor(key => key.Password)
            .NotNull().NotEmpty().WithMessage("Пароль не может быть пустым")
            .MinimumLength(5).WithMessage("Пароль должен быть более 5 символов")
            .MaximumLength(50).WithMessage("Пароль не может превышать 50 символов");

        RuleFor(key => key.Roles)
            .NotNull().NotEmpty().WithMessage("Роль пользователю нужно обязательно выбрать");
    }

    private static Task<bool> BeValidEmailAddress(string emailAddress, CancellationToken token)
    {
        var isValid = MyRegex().IsMatch(emailAddress);
        return Task.FromResult(isValid);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
    private static partial Regex MyRegex();
}