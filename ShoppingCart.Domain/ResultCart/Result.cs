namespace ShoppingCart.Domain.ResultCart
{
    public class Result
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }

        public List<string>? ValidationErrors { get; set; }
    }

    public class Result<Type> : Result
    {
        public Result(string? errorMessage, int? errorCode, Type? data)
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

        public Result(string? successMessage)
        {
            SuccessMessage = successMessage;
        }

        public Result(Type? data)
        {
            Data = data;
        }

        public Result()
        {

        }

        public Type? Data { get; set; }
    }
}