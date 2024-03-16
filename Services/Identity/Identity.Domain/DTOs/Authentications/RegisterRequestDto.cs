namespace Identity.Domain.DTOs.Authentications
{
    public class RegisterRequestDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string PasswordConfirm { get; set; } = null!;
    }
}