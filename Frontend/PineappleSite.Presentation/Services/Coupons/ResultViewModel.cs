namespace PineappleSite.Presentation.Services.Coupons;

public class ResultViewModel
{
    public bool IsSuccess => ErrorMessage == null;

    public string? SuccessMessage { get; init; }

    public string? ErrorMessage { get; init; }

    public int? StatusCode { get; init; }

    public string? ValidationErrors { get; init; }
}

public class ResultViewModel<T> : ResultViewModel
{
    public ResultViewModel(string? successMessage, int? statusCode, T? data)
    {
        SuccessMessage = successMessage;
        StatusCode = statusCode;
        Data = data;
    }

    public ResultViewModel(string? errorMessage, int? statusCode, T? data, string validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public ResultViewModel(string? errorMessage, int? statusCode, string? validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        ValidationErrors = validationErrors;
    }

    public ResultViewModel(string? successMessage)
    {
        SuccessMessage = successMessage;
    }

    public ResultViewModel(T? data)
    {
        Data = data;
    }

    public ResultViewModel()
    {
    }

    public T? Data { get; set; }
}