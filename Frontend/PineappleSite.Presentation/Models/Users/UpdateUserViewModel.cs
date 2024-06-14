using System.ComponentModel.DataAnnotations;
using static PineappleSite.Presentation.Utility.StaticDetails;

namespace PineappleSite.Presentation.Models.Users;

public sealed class UpdateUserViewModel
{
    public required string Id { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(2, ErrorMessage = "Имя должно быть более 2 символов")]
    [MaxLength(20, ErrorMessage = "Имя не может превышать 20 символов")]
    public required string FirstName { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(2, ErrorMessage = "Фамилия должна быть более 2 символов")]
    [MaxLength(20, ErrorMessage = "Фамилия не может превышать 20 символов")]
    public required string LastName { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [EmailAddress(ErrorMessage = "Введите действительный адрес электронной почты")]
    [MinLength(2, ErrorMessage = "Адрес электронной почты должен быть более 2 символов")]
    [MaxLength(50, ErrorMessage = "Адрес электронной почты не может превышать 50 символов")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Введите действительный адрес электронной почты")]
    public required string EmailAddress { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MinLength(5, ErrorMessage = "Длина строки имени пользователя должна быть более 5 символов")]
    [MaxLength(50, ErrorMessage = "Длина строки имени пользователя не должна превышать 50 символов")]
    public required string UserName { get; init; }

    [Required(ErrorMessage = "Выберите пользователю роль")]
    [Range(1, 2, ErrorMessage = "Выберите пользователю роль")]
    public UserRoles UserRoles { get; init; }
}