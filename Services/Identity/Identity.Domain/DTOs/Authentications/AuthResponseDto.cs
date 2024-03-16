namespace Identity.Domain.DTOs.Authentications
{
    public class AuthResponseDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UsertName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string JwtToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}