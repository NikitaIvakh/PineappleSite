namespace Identity.Domain.DTOs.Authentications
{
    public class TokenModelDto
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}