namespace Identity.Domain.DTOs.Authentications;

public sealed record RegisterRequestDto(
    string FirstName,
    string LastName,
    string EmailAddress,
    string UserName,
    string Password,
    string PasswordConfirm
);