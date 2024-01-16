using PineappleSite.Presentation.Services.Identities;
using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Users
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }

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

        public UserRoles UserRoles { get; set; }
    }
}