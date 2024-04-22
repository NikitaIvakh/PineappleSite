using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.DTOs.Identities;

public sealed record GetUserForUpdateDto(
    string UserId,
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    IEnumerable<IdentityRole<string>> Role,
    string Description,
    int? Age,
    string Password,
    string ImageUrl,
    string ImageLocalPath
);