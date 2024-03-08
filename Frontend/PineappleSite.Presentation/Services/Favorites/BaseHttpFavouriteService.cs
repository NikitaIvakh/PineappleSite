using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favourites;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Favorites
{
    public class BaseHttpFavouriteService(ILocalStorageService localStorageService, IFavoritesClient favoritesClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IFavoritesClient _favoritesClient = favoritesClient;

        protected FavouriteResult<FavouriteViewModel> ConvertCouponExceptions(FavoritesExceptions favoritesExceptions)
        {
            if (favoritesExceptions.StatusCode == 400)
            {
                return new FavouriteResult<FavouriteViewModel>() { ErrorMessage = "Произошли ошибки валидации.", ErrorCode = 400, ValidationErrors = favoritesExceptions.Response, };
            }

            else if (favoritesExceptions.StatusCode == 404)
            {
                return new FavouriteResult<FavouriteViewModel>() { ErrorMessage = "Требуемый элемент не удалось найти.", ErrorCode = 404 };
            }

            else
            {
                return new FavouriteResult<FavouriteViewModel>() { ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", ErrorCode = 500 };
            }
        }

        protected void AddBearerToken()
        {
            if (_localStorageService.Exists("token"))
            {
                _favoritesClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _localStorageService.GetStorageValue<string>("token"));
            }
        }
    }
}