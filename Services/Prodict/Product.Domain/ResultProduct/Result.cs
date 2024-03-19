namespace Product.Domain.ResultProduct
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

    public class Result<Type> : Result
    {
        public Result(string? successMessage, int? successCode, Type? data)
        {
            SuccessMessage = successMessage;
            SuccessCode = successCode;
            Data = data;
        }

        public Result(string? errorMessage, int? errorCode, Type? data, List<string> validationErrors)
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