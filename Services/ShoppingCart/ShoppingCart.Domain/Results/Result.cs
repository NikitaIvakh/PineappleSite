namespace ShoppingCart.Domain.Results
{
    public class Result
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }

        public int? ErrorCode { get; set; }

        public List<string>? ValidationErrors { get; set; }
    }

    public class Result<TEntity> : Result
    {
        public Result(string? errorMessage, int? errorCode, TEntity? data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public Result(string? errorMessage, int? errorCode, List<string>? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public Result(TEntity? data)
        {
            Data = data;
        }

        public Result()
        {

        }

        public TEntity? Data { get; set; }
    }
}