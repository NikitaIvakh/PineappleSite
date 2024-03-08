namespace PineappleSite.Presentation.Services.Orders
{
    public class OrderResult
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }

        public int? ErrorCode { get; set; }

        public string? ValidationErrors { get; set; }
    }

    public class OrderResult<TEntity> : OrderResult
    {
        public OrderResult(string? errorMessage, int? errorCode, TEntity? data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public OrderResult(string? errorMessage, int? errorCode, string? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
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