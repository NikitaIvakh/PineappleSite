namespace PineappleSite.Presentation.Services.Products
{
    public class ProductResultViewModel
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }

        public int? ErrorCode { get; set; }

        public int? SuccessCode { get; set; }

        public List<string>? ValidationErrors { get; set; }
    }

    public class ProductResultViewModel<Type> : ProductResultViewModel
    {
        public ProductResultViewModel(string? successMessage, int? successCode, Type? data)
        {
            SuccessMessage = successMessage;
            SuccessCode = successCode;
            Data = data;
        }

        public ProductResultViewModel(string? errorMessage, int? errorCode, Type? data, List<string> validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
            ValidationErrors = validationErrors;
        }

        public ProductResultViewModel(string? errorMessage, int? errorCode, List<string>? validationErrors)
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