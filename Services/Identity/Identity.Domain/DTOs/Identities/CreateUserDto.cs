using Identity.Domain.Enum;

namespace Identity.Domain.DTOs.Identities;

public sealed record CreateUserDto(
    string FirstName,
    string LastName,
    string Password,
    string UserName,
    string EmailAddress,
    UserRoles Roles
);