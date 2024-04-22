using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.DTOs.Identities;

public sealed record GetUsersDto(
    string UserId,
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    List<Task<IEnumerable<IdentityRole<string>>>> Role,
    DateTime? CreatedTime,
    DateTime? ModifiedTime
);