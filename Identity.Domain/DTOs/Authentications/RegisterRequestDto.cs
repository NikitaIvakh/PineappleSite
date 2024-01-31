using Identity.Domain.DTOs.Authentications;
using System.ComponentModel.DataAnnotations;

namespace Identity.Domain.DTOs.Authentications
{
    public class RegisterRequestDto : IRegisterRequestDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [MinLength(2)]
        public string UserName { get; set; }

        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}