namespace Identity.Domain.DTOs.Identities;

public sealed record GetUserForUpdateDto(
    string UserId,
    string? FirstName,
    string? LastName,
    string? UserName,
    string? EmailAddress,
    string? Description,
    int? Age,
    string? Password,
    string? ImageUrl,
    string? ImageLocalPath,
    IEnumerable<string> Role
);