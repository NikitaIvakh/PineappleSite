namespace Identity.Domain.DTOs.Authentications;

public sealed record TokenModelDto(string AccessToken, string RefreshToken);