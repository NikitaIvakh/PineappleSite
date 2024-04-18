namespace Coupon.Domain.ResultCoupon;

public class Result
{
    public bool IsSuccess => ErrorMessage == null;

    public string? SuccessMessage { get; init; }

    public string? ErrorMessage { get; init; }

    public int? StatusCode { get; init; }

    public List<string>? ValidationErrors { get; init; }
}

public class Result<T> : Result
{
    public Result(string? successMessage, T? data)
    {
        SuccessMessage = successMessage;
        Data = data;
    }

    public Result(string? errorMessage, int? statusCode, T? data, List<string> validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public Result(string? errorMessage, int? statusCode, List<string>? validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        ValidationErrors = validationErrors;
    }

    public Result(string? successMessage)
    {
        SuccessMessage = successMessage;
    }

    public Result(T? data)
    {
        Data = data;
    }

    public Result()
    {

    }

    public T? Data { get; set; }
}