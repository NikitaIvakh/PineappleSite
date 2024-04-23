namespace PineappleSite.Presentation.Models.Users;

public sealed record GetUserForUpdateViewModel(
    string UserId,
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    ICollection<string> Role,
    string Description,
    int? Age,
    string Password,
    string ImageUrl,
    string ImageLocalPath
);