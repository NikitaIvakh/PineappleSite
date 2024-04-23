namespace PineappleSite.Presentation.Models.Users;

public sealed record GetUsersViewModel(
    string UserId,
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    ICollection<string> Role,
    DateTimeOffset? CreatedTime,
    DateTimeOffset? ModifiedTime
);