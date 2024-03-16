namespace PineappleSite.Presentation.Models.Identities
{
    public class TokenModelViewModel
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}