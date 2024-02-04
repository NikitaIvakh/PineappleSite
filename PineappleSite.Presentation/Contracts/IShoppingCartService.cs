using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartResultViewModel<CartViewModel>> GetShoppingCartAsync(string userId);

        Task<CartResultViewModel<CartHeaderViewModel>> CartUpsertAsync(CartViewModel cartViewModel);

        Task<CartResultViewModel<CartHeaderViewModel>> ApplyCouponAsync(CartViewModel cartViewModel);

        Task<CartResultViewModel<CartHeaderViewModel>> RemoveCouponAsync(CartViewModel cartViewModel);

        Task<CartResultViewModel<CartDetailsViewModel>> RemoveCartDetailsAsync(int cartDEtailsId);
    }
}