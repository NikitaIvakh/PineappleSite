namespace PineappleSite.Presentation.Models.Users;

public record GetUsersProfileViewModel(
    string UserId,
    string? FirstName,
    string? LastName,
    string? UserName,
    string? EmailAddress,
    string? Description,
    int? Age,
    string? ImageUrl,
    string? ImageLocalPath,
    DateTimeOffset? CreatedTime,
    DateTimeOffset? ModifiedTime,
    ICollection<string> Role
);