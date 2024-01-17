namespace Identity.Application.DTOs.Identities
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
    }
}