namespace Identity.Domain.DTOs.Identities;

public sealed record GetUserDto(
    string UserId,
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    DateTime? CreatedTime,
    DateTime? ModifiedTime,
    IEnumerable<string> Role
);