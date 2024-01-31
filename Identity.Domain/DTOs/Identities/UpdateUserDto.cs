using Identity.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Identity.Domain.DTOs.Identities
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