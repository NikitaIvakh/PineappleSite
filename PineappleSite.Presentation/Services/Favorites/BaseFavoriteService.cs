using PineappleSite.Presentation.Contracts;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Favorites
{
    public class BaseFavoriteService(ILocalStorageService localStorageService, IFavoritesClient favoritesClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IFavoritesClient _favoritesClient = favoritesClient;

        protected FavoritesResponseViewModel ConvertFavoriteExceptions(FavoritesExceptions favoritesExceptions)
        {
            if (favoritesExceptions.StatusCode == 400)
            {
                return new FavoritesResponseViewModel() { Message = "Произошли ошибки валидации.", ValidationErrors = favoritesExceptions.Response, IsSuccess = false };
            }

            else if (favoritesExceptions.StatusCode == 404)
            {
                return new FavoritesResponseViewModel() { Message = "Требуемый элемент не удалось найти.", IsSuccess = false };
            }

            else
            {
                return new FavoritesResponseViewModel() { Message = "Что-то пошло не так, пожалуйста, попробуйте еще раз.", IsSuccess = false };
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