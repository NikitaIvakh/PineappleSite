namespace PineappleSite.Presentation.Models.Identities
{
    public class AuthRequestViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}