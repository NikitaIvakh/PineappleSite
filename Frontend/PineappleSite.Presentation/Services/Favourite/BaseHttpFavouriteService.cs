using System.Net.Http.Headers;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favourites;

namespace PineappleSite.Presentation.Services.Favourite;

public class BaseHttpFavouriteService(
    ILocalStorageService localStorageService,
    IFavouriteClient favouriteClient,
    IHttpContextAccessor httpContextAccessor)
{
    protected static FavouriteResult<FavouriteViewModel> ConvertCouponExceptions(
        FavouriteExceptions<string> favoritesExceptions)
    {
        return favoritesExceptions.StatusCode switch
        {
            403 => new FavouriteResult<FavouriteViewModel>
            {
                StatusCode = 403,
                ErrorMessage =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам.",
                ValidationErrors =
                    "Пользователям не доступна эта страница. Эта страница доступна только администраторам."
            },

            401 => new FavouriteResult<FavouriteViewModel>
            {
                StatusCode = 401,
                ErrorMessage = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться.",
                ValidationErrors = "Чтобы получить доступ к ресурсу, необходимо зарегистрироваться."
            },

            _ => new FavouriteResult<FavouriteViewModel>()
            {
                StatusCode = 500,
                ErrorMessage = favoritesExceptions.Result,
                ValidationErrors = favoritesExceptions.Result
            }
        };
    }

    protected void AddBearerToken()
    {
        if (httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey("JWTToken"))
        {
            favouriteClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                httpContextAccessor.HttpContext.Request.Cookies["JWTToken"]);
        }
    }
}