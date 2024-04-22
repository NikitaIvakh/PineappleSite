namespace Identity.Domain.DTOs.Authentications;

public sealed record AuthResponseDto(
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    string JwtToken,
    string RefreshToken
);