namespace PineappleSite.Presentation.Services.Orders;

public class OrderResult
{
    public bool IsSuccess => ErrorMessage == null;

    public string? SuccessMessage { get; init; }

    public string? ErrorMessage { get; init; }

    public int? StatusCode { get; init; }

    public string? ValidationErrors { get; init; }
}

public class OrderResult<T> : OrderResult
{
    public OrderResult(string? successMessage, T? data)
    {
        SuccessMessage = successMessage;
        Data = data;
    }

    public OrderResult(string? errorMessage, int? statusCode, T? data, string validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public OrderResult(string? errorMessage, int? statusCode, string? validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        ValidationErrors = validationErrors;
    }

    public OrderResult(string? successMessage)
    {
        SuccessMessage = successMessage;
    }

    public OrderResult(T? data)
    {
        Data = data;
    }

    public OrderResult()
    {

    }

    public T? Data { get; set; }
}