using FluentValidation;
using Identity.Domain.DTOs.Identities;
using System.Text.RegularExpressions;

namespace Identity.Application.Validators;

public sealed partial class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(key => key.Id)
            .NotNull().NotEmpty().WithMessage("Идентификатор не может быть пустым");

        RuleFor(dto => dto.FirstName)
            .NotNull().NotEmpty().WithMessage("Имя не может быть пустым")
            .MaximumLength(20).WithMessage("Имя не может превышать 20 символов");

        RuleFor(dto => dto.LastName)
            .NotNull().NotEmpty().WithMessage("Фамилия не может быть пустой")
            .MaximumLength(20).WithMessage("Фамилия не может превышать 20 символов");

        RuleFor(dto => dto.UserName)
            .NotNull().NotEmpty().WithMessage("Имя пользователя не может быть пустым")
            .When(dto => !string.IsNullOrEmpty(dto.UserName))
            .MaximumLength(50).WithMessage("Длина строки имени пользователя не должна превышать 50 символов")
            .MustAsync(BeUniqueUserName).WithMessage("Такое имя пользователя уже существует");

        RuleFor(dto => dto.EmailAddress)
            .NotNull().NotEmpty().WithMessage("Адрес электронной почты не может быть пустым")
            .When(dto => !string.IsNullOrEmpty(dto.EmailAddress))
            .MinimumLength(2).WithMessage("Адрес электронной почты должен быть более 2 символов")
            .MaximumLength(50).WithMessage("Адрес электронной почты не может превышать 50 символов")
            .MustAsync(BeUniqueEmailAddress).WithMessage("Такой адрес электронной почты уже используется!")
            .MustAsync(BeValidEmailAddress).WithMessage("Введите действительный адрес электронной почты");
    }

    private static Task<bool> BeUniqueUserName(string userName, CancellationToken token)
    {
        return Task.FromResult(string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(userName));
    }

    private static Task<bool> BeUniqueEmailAddress(string emailAddress, CancellationToken token)
    {
        return Task.FromResult(string.IsNullOrEmpty(emailAddress) || !string.IsNullOrEmpty(emailAddress));
    }

    private static Task<bool> BeValidEmailAddress(string emailAddress, CancellationToken token)
    {
        var isValid = MyRegex().IsMatch(emailAddress);
        return Task.FromResult(isValid);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
    private static partial Regex MyRegex();
}