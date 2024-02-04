namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public class CartResultViewModel
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }

        public string? ValidationErrors { get; set; }
    }

    public class CartResultViewModel<Type> : CartResultViewModel
    {
        public CartResultViewModel(string? errorMessage, int? errorCode, Type? data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public CartResultViewModel(string? errorMessage, int? errorCode, string? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public CartResultViewModel(string? successMessage)
        {
            SuccessMessage = successMessage;
        }

        public CartResultViewModel(Type? data)
        {
            Data = data;
        }

        public CartResultViewModel()
        {

        }

        public Type? Data { get; set; }
    }
}