using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Identities;

public sealed class RegisterRequestViewModel
{
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(2, ErrorMessage = "Имя должно быть более 2 символов")]
    [MaxLength(20, ErrorMessage = "Имя не может превышать 20 символов")]
    public required string FirstName { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(2, ErrorMessage = "Фамилия должна быть более 2 символов")]
    [MaxLength(20, ErrorMessage = "Фамилия не может превышать 20 символов")]
    public required string LastName { get; init; }

    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Ведите валидный адрес электронной почты")]
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(5, ErrorMessage = "Длина строки имени пользователя должна быть более 5 символов")]
    [MaxLength(50, ErrorMessage = "Длина строки имени пользователя не должна превышать 50 символов")]
    public required string EmailAddress { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(2, ErrorMessage = "Адрес электронной почты должен быть более 2 символов")]
    [MaxLength(50, ErrorMessage = "Адрес электронной почты не может превышать 50 символов")]
    public required string UserName { get; init; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(5, ErrorMessage = "Пароль должен быть более 5 символов")]
    [MaxLength(50, ErrorMessage = "Пароль не может превышать 50 символов")]
    public required string Password { get; init; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(5, ErrorMessage = "Пароль должен быть более 5 символов")]
    [MaxLength(50, ErrorMessage = "Пароль не может превышать 50 символов")]
    public required string PasswordConfirm { get; init; }
}