using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Identities
{
    public class RegisterRequestViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MinLength(2)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Password { get; set; }
    }
}