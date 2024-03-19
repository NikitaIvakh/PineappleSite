namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public class CartResult
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }

        public int? ErrorCode { get; set; }

        public int? SuccessCode { get; set; }

        public List<string>? ValidationErrors { get; set; }
    }

    public class CartResult<TEntity> : CartResult
    {
        public CartResult(string? successMessage, int? successCode, TEntity? data)
        {
            SuccessMessage = successMessage;
            SuccessCode = successCode;
            Data = data;
        }

        public CartResult(string? errorMessage, int? errorCode, TEntity? data, List<string> validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
            ValidationErrors = validationErrors;
        }

        public CartResult(string? errorMessage, int? errorCode, List<string>? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public CartResult(string? successMessage)
        {
            SuccessMessage = successMessage;
        }

        public CartResult(TEntity? data)
        {
            Data = data;
        }

        public CartResult()
        {

        }

        public TEntity? Data { get; set; }
    }
}