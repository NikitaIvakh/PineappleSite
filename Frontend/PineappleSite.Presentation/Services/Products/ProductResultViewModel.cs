namespace PineappleSite.Presentation.Services.Products;

public class ProductResultViewModel
{
    public bool IsSuccess => ErrorMessage == null;

    public string? SuccessMessage { get; init; }

    public string? ErrorMessage { get; init; }

    public int? StatusCode { get; init; }

    public string? ValidationErrors { get; init; }
}

public class ProductResultViewModel<T> : ProductResultViewModel
{
    public ProductResultViewModel(string? successMessage, T? data)
    {
        SuccessMessage = successMessage;
        Data = data;
    }

    public ProductResultViewModel(string? errorMessage, int? statusCode, T? data, string validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public ProductResultViewModel(string? errorMessage, int? statusCode, string? validationErrors)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
        ValidationErrors = validationErrors;
    }

    public ProductResultViewModel(string? successMessage)
    {
        SuccessMessage = successMessage;
    }

    public ProductResultViewModel(T? data)
    {
        Data = data;
    }

    public ProductResultViewModel()
    {
    }

    public T? Data { get; set; }
}