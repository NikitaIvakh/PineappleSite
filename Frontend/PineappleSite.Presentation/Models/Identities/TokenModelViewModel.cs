namespace PineappleSite.Presentation.Models.Identities;

public class TokenModelViewModel
{
    public string AccessToken { get; init; } = null!;

    public string RefreshToken { get; init; } = null!;
}