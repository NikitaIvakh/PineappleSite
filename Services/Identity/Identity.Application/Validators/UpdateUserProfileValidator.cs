using FluentValidation;
using System.Text.RegularExpressions;
using Identity.Domain.DTOs.Identities;

namespace Identity.Application.Validators;

public sealed partial class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileDto>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(key => key.Id)
            .NotNull().NotEmpty().WithMessage("Идентификатор не может быть пустым");

        RuleFor(dto => dto.FirstName)
            .MinimumLength(2).WithMessage("Имя должно быть более 2 символов")
            .MaximumLength(20).WithMessage("Имя не может превышать 20 символов");

        RuleFor(dto => dto.LastName)
            .MinimumLength(2).WithMessage("Фамилия должна быть более 2 символов")
            .MaximumLength(20).WithMessage("Фамилия не может превышать 20 символов");

        RuleFor(dto => dto.UserName)
            .MinimumLength(5).WithMessage("Длина строки имени пользователя должна быть более 5 символов")
            .MaximumLength(50).WithMessage("Длина строки имени пользователя не должна превышать 50 символов")
            .MustAsync(BeUniqieUserName).WithMessage("Такое имя пользователя уже существует");

        RuleFor(dto => dto.EmailAddress)
            .MinimumLength(2).WithMessage("Адрес электронной почты должен быть более 2 символов")
            .MaximumLength(50).WithMessage("Адрес электронной почты не может превышать 50 символов")
            .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");

        RuleFor(key => key.Description)
            .MinimumLength(10).WithMessage("Описание должно быть более 10 символов")
            .MaximumLength(300).WithMessage("Описание не может превышать 300 символов");

        RuleFor(key => key.Age)
            .GreaterThanOrEqualTo(18).WithMessage("Возраст должен быть больше 18")
            .LessThanOrEqualTo(120).WithMessage("Возраст не должен превышать 120");

        RuleFor(key => key.Password)
            .MinimumLength(3).WithMessage("Длина пароля должна быть более 3 символов")
            .MaximumLength(30).WithMessage("Длина пароля не может превышать 30 символов");
    }

    private static Task<bool> BeUniqieUserName(string userName, CancellationToken arg2)
    {
        return Task.FromResult(string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userName));
    }

    private static Task<bool> BeValidEmailAddress(string emailAddress, CancellationToken token)
    {
        var isValid = MyRegex().IsMatch(emailAddress);
        return Task.FromResult(isValid);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
    private static partial Regex MyRegex();
}