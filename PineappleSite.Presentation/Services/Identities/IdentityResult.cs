namespace PineappleSite.Presentation.Services.Identities
{
    public class IdentityResult
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }

        public string? ValidationErrors { get; set; }
    }

    public class IdentityResult<Type> : IdentityResult
    {
        public IdentityResult(string? errorMessage, int? errorCode, Type? data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public IdentityResult(string? errorMessage, int? errorCode, string? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public IdentityResult(string? successMessage)
        {
            SuccessMessage = successMessage;
        }

        public IdentityResult(Type? data)
        {
            Data = data;
        }

        public IdentityResult()
        {

        }

        public Type? Data { get; set; }
    }
}