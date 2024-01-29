namespace PineappleSite.Presentation.Services.Favorites
{
    public class FavouriteResultViewModel
    {
        public bool IsSuccess => ErrorMessage == null;

        public string? ErrorMessage { get; set; }

        public string? SuccessMessage { get; set; }

        public int? ErrorCode { get; set; }

        public string? ValidationErrors { get; set; }
    }

    public class FavouriteResultViewModel<TEntity> : FavouriteResultViewModel
    {
        public FavouriteResultViewModel(string? errorMessage, int? errorCode, TEntity? data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public FavouriteResultViewModel(string? errorMessage, int? errorCode, string? validationErrors)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public FavouriteResultViewModel(TEntity? data)
        {
            Data = data;
        }

        public FavouriteResultViewModel()
        {

        }

        public TEntity? Data { get; set; }
    }
}