namespace PineappleSite.Presentation.Services.Favorites
{
    public class FavouriteResult
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }

        public int? ErrorCode { get; set; }

        public List<string>? ValidationErrors { get; set; }
    }

    public class FavouriteResult<TEntity> : FavouriteResult
    {
        public FavouriteResult(string? errorMessage, int? errorCode, TEntity? data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public FavouriteResult(string? errorMessage, int? errorCode, List<string>? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public FavouriteResult(TEntity? data)
        {
            Data = data;
        }

        public FavouriteResult()
        {

        }

        public TEntity? Data { get; set; }
    }
}