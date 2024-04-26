namespace PineappleSite.Presentation.Services.ShoppingCarts;

public class CartResult
{
    public bool IsSuccess => ErrorMessage == null;

    public string? SuccessMessage { get; init; }

    public string? ErrorMessage { get; init; }

    public int? StatusCode { get; init; }

    public string? ValidationErrors { get; init; }
}

public class CartResult<T> : CartResult
{
    public CartResult(string? successMessage, T? data)
    {
        SuccessMessage = successMessage;
        Data = data;
    }

    public CartResult(string? errorMessage, int? statusCode, T? data, string validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public CartResult(string? errorMessage, int? statusCode, string? validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        ValidationErrors = validationErrors;
    }

    public CartResult(string? successMessage)
    {
        SuccessMessage = successMessage;
    }

    public CartResult(T? data)
    {
        Data = data;
    }

    public CartResult()
    {
    }

    public T? Data { get; set; }
}