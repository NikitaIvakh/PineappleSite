namespace Identity.Application.DTOs.Authentications
{
    public class AuthResponseDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}