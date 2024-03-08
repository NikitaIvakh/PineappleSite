using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Identities
{
    public class AuthRequestViewModel
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Ведите валидный адрес электронной почты")]
        [Required(ErrorMessage = "Адрес электронной почты не может быть пустым")]
        [MinLength(2, ErrorMessage = "Адрес электронной почты должен быть более 2 символов")]
        [MaxLength(50, ErrorMessage = "Адрес электронной почты не может превышать 50 символов")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(5, ErrorMessage = "Пароль должен быть более 5 символов")]
        [MaxLength(50, ErrorMessage = "Пароль не может превышать 50 символов")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}