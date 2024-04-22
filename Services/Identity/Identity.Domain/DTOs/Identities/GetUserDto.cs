using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.DTOs.Identities;

public sealed record GetUserDto(
    string UserId,
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    IEnumerable<IdentityRole<string>> Role,
    DateTime? CreatedTime,
    DateTime? ModifiedTime
);