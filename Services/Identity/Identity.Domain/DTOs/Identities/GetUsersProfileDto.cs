namespace Identity.Domain.DTOs.Identities;

public record GetUsersProfileDto(
    string UserId,
    string? FirstName,
    string? LastName,
    string? UserName,
    string? EmailAddress,
    string? Description,
    int? Age,
    string? ImageUrl,
    string? ImageLocalPath,
    DateTime? CreatedTime,
    DateTime? ModifiedTime,
    IEnumerable<string> Role
);