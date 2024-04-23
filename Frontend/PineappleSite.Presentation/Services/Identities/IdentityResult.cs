namespace PineappleSite.Presentation.Services.Identities;

public class IdentityResult
{
    public bool IsSuccess => ErrorMessage == null;

    public string? SuccessMessage { get; init; }

    public string? ErrorMessage { get; init; }

    public int? StatusCode { get; init; }

    public string? ValidationErrors { get; init; }
}

public class IdentityResult<T> : IdentityResult
{
    public IdentityResult(string? successMessage, T? data)
    {
        SuccessMessage = successMessage;
        Data = data;
    }

    public IdentityResult(string? errorMessage, int? statusCode, T? data, string validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public IdentityResult(string? errorMessage, int? statusCode, string? validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        ValidationErrors = validationErrors;
    }

    public IdentityResult(string? successMessage)
    {
        SuccessMessage = successMessage;
    }

    public IdentityResult(T? data)
    {
        Data = data;
    }

    public IdentityResult()
    {
    }

    public T? Data { get; set; }
}