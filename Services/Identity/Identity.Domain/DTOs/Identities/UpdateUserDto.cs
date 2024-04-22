using Identity.Domain.Enum;

namespace Identity.Domain.DTOs.Identities;

public sealed record UpdateUserDto(
    string Id,
    string FirstName,
    string LastName,
    string EmailAddress,
    string UserName,
    UserRoles UserRoles
);