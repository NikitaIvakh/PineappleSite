namespace Identity.Domain.DTOs.Authentications;

public sealed record AuthRequestDto(string EmailAddress, string Password);