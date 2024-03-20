using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favourites;
using System.Net.Http.Headers;

namespace PineappleSite.Presentation.Services.Favorites
{
    public class BaseHttpFavouriteService(ILocalStorageService localStorageService, IFavoritesClient favoritesClient, IHttpContextAccessor httpContextAccessor)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IFavoritesClient _favoritesClient = favoritesClient;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected FavouriteResult<FavouriteViewModel> ConvertCouponExceptions(FavoritesExceptions favoritesExceptions)
        {
            if (favoritesExceptions.StatusCode == 403)
            {
                return new FavouriteResult<FavouriteViewModel>
                {
                    ErrorCode = 403,
                    ErrorMessage = "Пользователям не доступна эта страница. Это страница доступна только администраторам.",
                    ValidationErrors = ["Пользователям не доступна эта страница. Эта страница доступна только администраторам."]
                };
            }

            else if (favoritesExceptions.StatusCode == 401)
            {
                return new FavouriteResult<FavouriteViewModel>
                {
                    ErrorCode = 401,
                    ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                    ValidationErrors = ["Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."]
                };
            }

            else
            {
                return new FavouriteResult<FavouriteViewModel>()
                {
                    ErrorCode = 500,
                    ErrorMessage = "Что-то пошло не так, пожалуйста, попробуйте еще раз.",
                    ValidationErrors = ["Что-то пошло не так, пожалуйста, попробуйте еще раз."]
                };
            }
        }

        protected void AddBearerToken()
        {
            if (_httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
            {
                _favoritesClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Cookies["JWTToken"]);
            }
        }
    }
}