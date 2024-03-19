namespace PineappleSite.Presentation.Services.Orders
{
    public class OrderResult
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }

        public int? ErrorCode { get; set; }

        public int? SuccessCode { get; set; }

        public List<string>? ValidationErrors { get; set; }
    }

    public class OrderResult<TEntity> : OrderResult
    {
        public OrderResult(string? successMessage, int? successCode, TEntity? data)
        {
            SuccessMessage = successMessage;
            SuccessCode = successCode;
            Data = data;
        }

        public OrderResult(string? errorMessage, int? errorCode, TEntity? data, List<string> validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
            ValidationErrors = validationErrors;
        }

        public OrderResult(string? errorMessage, int? errorCode, List<string>? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public OrderResult(string? successMessage)
        {
            SuccessMessage = successMessage;
        }

        public OrderResult(TEntity? data)
        {
            Data = data;
        }

        public OrderResult()
        {

        }

        public TEntity? Data { get; set; }
    }
}