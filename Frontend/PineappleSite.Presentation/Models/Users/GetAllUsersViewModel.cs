﻿namespace PineappleSite.Presentation.Models.Users
{
    public record class GetAllUsersViewModel(string UserId, string FirstName, string LastName, string UserName, string EmailAddress, ICollection<string> Role, DateTimeOffset? CreatedTime, DateTimeOffset? ModifiedTime);
}