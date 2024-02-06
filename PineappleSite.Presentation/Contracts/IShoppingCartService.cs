using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartResult<CartViewModel>> GetCartAsync(string userId);

        Task<CartResult<CartViewModel>> CartUpsertAsync(CartViewModel cartViewModel);

        Task<CartResult<CartViewModel>> ApplyCouponAsync(CartViewModel cartViewModel);

        Task<CartResult<CartViewModel>> RemoveCouponAsync(CartViewModel cartViewModel);

        Task<CartResult<CartViewModel>> RemoveCartDetailsAsync(int cartDetailsId);
    }
}