namespace Identity.Application.Response
{
    public class BaseIdentityResponse
    {
        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = true;

        public List<string> ValidationErrors { get; set; }

        public object? Data { get; set; }
    }
}
