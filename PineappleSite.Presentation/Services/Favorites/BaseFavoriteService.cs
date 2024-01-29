using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favorites;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Favorites
{
    public class BaseFavoriteService(ILocalStorageService localStorageService, IFavoritesClient favoritesClient)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IFavoritesClient _favoritesClient = favoritesClient;

        protected FavouriteResultViewModel<FavouritesViewModel> ConvertFavoriteExceptions(FavoritesExceptions favoritesExceptions)
        {
            if (favoritesExceptions.StatusCode == 400)
            {
                return new FavouriteResultViewModel<FavouritesViewModel>() { ErrorMessage = "Произошли ошибки валидации.", ValidationErrors = favoritesExceptions.Response };
            }

            else if (favoritesExceptions.StatusCode == 404)
            {
                return new FavouriteResultViewModel<FavouritesViewModel>() { ErrorMessage = "Требуемый элемент не удалось найти." };
            }

            else
            {
                return new FavouriteResultViewModel<FavouritesViewModel>() { ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз." };
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