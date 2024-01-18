using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Models.Users
{
    public class UpdateUserProfileViewModel
    {
        public string Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? EmailAddress { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Description { get; set; }

        public int? Age { get; set; }

        public IList<string> Roles { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }
    }
}