namespace PineappleSite.Presentation.Models.Identities;

public sealed class RegisterResponseViewModel
{
    public string? Id { get; init; }

    public string? UserName { get; init; }

    public string? Email { get; init; }

    public string? Token { get; init; }
}