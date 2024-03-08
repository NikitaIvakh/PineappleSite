using Microsoft.AspNetCore.Http;

namespace Identity.Domain.DTOs.Identities
{
    public class UpdateUserProfileDto
    {
        public string Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? EmailAddress { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Description { get; set; }

        public int? Age { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }
    }
}