namespace PineappleSite.Presentation.Services.Coupons
{
    public class ResultViewModel
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? SuccessMessage { get; set; }

        public string? ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }

        public int? SuccessCode { get; set; }

        public List<string>? ValidationErrors { get; set; }
    }

    public class ResultViewModel<Type> : ResultViewModel
    {
        public ResultViewModel(string? successMessage, int? successCode, Type? data)
        {
            SuccessMessage = successMessage;
            SuccessCode = successCode;
            Data = data;
        }

        public ResultViewModel(string? errorMessage, int? errorCode, Type? data, List<string> validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
            ValidationErrors = validationErrors;
        }

        public ResultViewModel(string? errorMessage, int? errorCode, List<string>? validationErrors)
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