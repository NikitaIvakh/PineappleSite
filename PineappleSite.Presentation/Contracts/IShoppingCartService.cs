using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartResultViewModel<CartViewModel>> GetShoppingCartAsync(string userId);

        Task<CartResultViewModel<CartViewModel>> CartUpsertAsync(CartViewModel cartViewModel);

        Task<CartResultViewModel<CartViewModel>> ApplyCouponAsync(CartViewModel cartViewModel);

        Task<CartResultViewModel<CartViewModel>> RemoveCouponAsync(CartViewModel cartViewModel);

        Task<CartResultViewModel<CartViewModel>> RemoveCartDetailsAsync(int cartDEtailsId);
    }
}