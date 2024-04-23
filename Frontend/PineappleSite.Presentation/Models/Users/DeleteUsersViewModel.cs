namespace PineappleSite.Presentation.Models.Users;

public sealed class DeleteUsersViewModel(List<string> userIds)
{
    public List<string> UserIds { get; set; } = userIds;
}