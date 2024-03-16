namespace Identity.Domain.DTOs.Authentications
{
    public class AuthRequestDto
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}