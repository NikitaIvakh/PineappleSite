namespace PineappleSite.Presentation.Services.Products
{
    public class ProductResultViewModel
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }

        public string? ValidationErrors { get; set; }
    }

    public class ProductResultViewModel<Type> : ProductResultViewModel
    {
        public ProductResultViewModel(string? errorMessage, int? errorCode, Type? data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public ProductResultViewModel(string? errorMessage, int? errorCode, string? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public ProductResultViewModel(string? successMessage)
        {
            SuccessMessage = successMessage;
        }

        public ProductResultViewModel(Type? data)
        {
            Data = data;
        }

        public ProductResultViewModel()
        {

        }

        public Type? Data { get; set; }
    }
}