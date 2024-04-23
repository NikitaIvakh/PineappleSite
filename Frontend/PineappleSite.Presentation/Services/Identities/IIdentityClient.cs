namespace PineappleSite.Presentation.Services.Identities;

public partial interface IIdentityClient
{
    public HttpClient HttpClient { get; }
}