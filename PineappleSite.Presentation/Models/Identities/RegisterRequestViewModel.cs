using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Identities
{
    public class RegisterRequestViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [MinLength(2)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}