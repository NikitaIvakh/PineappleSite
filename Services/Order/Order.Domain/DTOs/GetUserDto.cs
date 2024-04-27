namespace Order.Domain.DTOs;

public sealed record class GetUserDto(
    string UserId,
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    DateTime? CreatedTime,
    DateTime? ModifiedTime,
    IEnumerable<string> Role
);