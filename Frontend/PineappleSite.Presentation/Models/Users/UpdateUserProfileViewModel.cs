using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Users;

public sealed class UpdateUserProfileViewModel
{
    public string? Id { get; init; }

    [MinLength(2, ErrorMessage = "Имя должно быть более 2 символов")]
    [MaxLength(20, ErrorMessage = "Имя не может превышать 20 символов")]
    public string? FirstName { get; init; }

    [MinLength(2, ErrorMessage = "Фамилия должна быть более 2 символов")]
    [MaxLength(20, ErrorMessage = "Фамилия не может превышать 20 символов")]
    public string? LastName { get; init; }

    [EmailAddress(ErrorMessage = "Введите действительный адрес электронной почты")]
    [MinLength(2, ErrorMessage = "Адрес электронной почты должен быть более 2 символов")]
    [MaxLength(50, ErrorMessage = "Адрес электронной почты не может превышать 50 символов")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Введите действительный адрес электронной почты")]
    public string? EmailAddress { get; init; }

    [MinLength(5, ErrorMessage = "Длина строки имени пользователя должна быть более 5 символов")]
    [MaxLength(50, ErrorMessage = "Длина строки имени пользователя не должна превышать 50 символов")]
    public string? UserName { get; init; }

    [MinLength(3, ErrorMessage = "Длина пароля должна быть более 3 символов")]
    [MaxLength(30, ErrorMessage = "Длина пароля не может превышать 30 символов")]
    public string? Password { get; init; }

    [MinLength(10, ErrorMessage = "Описание должно быть более 10 символов")]
    [MaxLength(300, ErrorMessage = "Описание не может превышать 300 символов")]
    public string? Description { get; init; }

    [Range(18, 120, ErrorMessage = "Возраст должен быть больше 18, но меньше 120")]
    public int? Age { get; init; }
    
    public ICollection<string>? Roles { get; init; }

    public IFormFile? Avatar { get; set; }

    public string? ImageUrl { get; init; }

    public string? ImageLocalPath { get; init; }
}