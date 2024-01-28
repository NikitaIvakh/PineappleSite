namespace PineappleSite.Presentation.Services.Coupons
{
    public class ResultViewModel
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }

        public string? ValidationErrors { get; set; }
    }

    public class ResultViewModel<Type> : ResultViewModel
    {
        public ResultViewModel(string? errorMessage, int? errorCode, Type? data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public ResultViewModel(string? errorMessage, int? errorCode, string? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public ResultViewModel(string? successMessage)
        {
            SuccessMessage = successMessage;
        }

        public ResultViewModel(Type? data)
        {
            Data = data;
        }

        public ResultViewModel()
        {

        }

        public Type? Data { get; set; }
    }
}