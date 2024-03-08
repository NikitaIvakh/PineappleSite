namespace PineappleSite.Presentation.Services.Identities
{
    public partial class IdentityClient : IIdentityClient
    {
        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }
    }
}