namespace PineappleSite.Presentation.Services.Favourite;

public class FavouriteResult
{
    public bool IsSuccess => ErrorMessage == null;

    public string? SuccessMessage { get; init; }

    public string? ErrorMessage { get; init; }

    public int? StatusCode { get; init; }

    public string? ValidationErrors { get; init; }
}

public class FavouriteResult<T> : FavouriteResult
{
    public FavouriteResult(string? successMessage, T? data)
    {
        SuccessMessage = successMessage;
        Data = data;
    }

    public FavouriteResult(string? errorMessage, int? statusCode, T? data, string validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public FavouriteResult(string? errorMessage, int? statusCode, string? validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        ValidationErrors = validationErrors;
    }

    public FavouriteResult(string? successMessage)
    {
        SuccessMessage = successMessage;
    }

    public FavouriteResult(T? data)
    {
        Data = data;
    }

    public FavouriteResult()
    {
    }

    public T? Data { get; set; }
}