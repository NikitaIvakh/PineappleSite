namespace Identity.Domain.DTOs.Identities;

public sealed record GetUsersDto(
    string UserId,
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    IEnumerable<Task<IList<string>>> Role,
    DateTime? CreatedTime,
    DateTime? ModifiedTime
);