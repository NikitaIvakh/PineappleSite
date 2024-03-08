using PineappleSite.Presentation.Services.Identities;
using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Users
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(2, ErrorMessage = "Имя должно быть более 2 символов")]
        [MaxLength(20, ErrorMessage = "Имя не может превышать 20 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(2, ErrorMessage = "Фамилия должна быть более 2 символов")]
        [MaxLength(20, ErrorMessage = "Фамилия не может превышать 20 символов")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(5, ErrorMessage = "Пароль должн быть более 5 символов")]
        [MaxLength(50, ErrorMessage = "Пароль не может превышать 50 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(2, ErrorMessage = "Адрес электронной почты должн быть более 2 символов")]
        [MaxLength(50, ErrorMessage = "Адрес электронной почты не может превышать 50 символов")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Ведите валидный адрес электронной почты")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(5, ErrorMessage = "Длина строки имени пользователя должна быть более 5 символов")]
        [MaxLength(50, ErrorMessage = "Длина строки имени пользователя не должна превышать 50 символов")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public UserRoles Roles { get; set; }
    }
}