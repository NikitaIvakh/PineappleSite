namespace Identity.Core.Response
{
    public class BaseIdentityResponse<TData>
    {
        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public List<string> ValidationErrors { get; set; }

        public TData? Data { get; set; }
    }
}
