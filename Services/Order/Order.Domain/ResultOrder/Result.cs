namespace Order.Domain.ResultOrder
{
    public class Result
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }

        public int? ErrorCode { get; set; }

        public int? SuccessCode { get; set; }

        public List<string>? ValidationErrors { get; set; }
    }

    public class Result<TEntity> : Result
    {
        public Result(string? successMessage, int? successCode, TEntity? data)
        {
            SuccessMessage = successMessage;
            SuccessCode = successCode;
            Data = data;
        }

        public Result(string? errorMessage, int? errorCode, TEntity? data, List<string> validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
            ValidationErrors = validationErrors;
        }

        public Result(string? errorMessage, int? errorCode, List<string>? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public Result(string? successMessage)
        {
            SuccessMessage = successMessage;
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