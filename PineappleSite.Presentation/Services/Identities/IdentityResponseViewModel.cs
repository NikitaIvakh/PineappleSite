namespace PineappleSite.Presentation.Services.Identities
{
    public class IdentityResponseViewModel
    {
        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string ValidationErrors { get; set; }

        public object? Data { get; set; }
    }
}