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

    public class CartResultViewModel<TEntity> : CartResultViewModel
    {
        public CartResultViewModel(string? errorMessage, int? errorCode, TEntity? data)
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

        public CartResultViewModel(TEntity? data)
        {
            Data = data;
        }

        public CartResultViewModel()
        {

        }

        public TEntity? Data { get; set; }
    }
}