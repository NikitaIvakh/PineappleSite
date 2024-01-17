using System.ComponentModel.DataAnnotations;
using static Identity.Application.Utilities.StaticDetails;

namespace Identity.Application.DTOs.Identities
{
    public class UpdateUserDto
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

        public UserRoles UserRoles { get; set; }
    }
}