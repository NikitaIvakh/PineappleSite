namespace PineappleSite.Presentation.Models.Users;

public sealed class DeleteUserViewModel(string id)
{
    public string Id { get; set; } = id;
}