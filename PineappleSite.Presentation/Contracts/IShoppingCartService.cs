using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.Contracts
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartResponseViewModel> GetShoppingCartAsync(string userId);

        Task<ShoppingCartResponseViewModel> CartUpsertAsync(CartViewModel cartViewModel);

        Task<ShoppingCartResponseViewModel> ApplyCouponAsync(CartViewModel cartViewModel);

        Task<ShoppingCartResponseViewModel> RemoveCouponAsync(CartViewModel cartViewModel);

        Task<ShoppingCartResponseViewModel> RemoveCartDetailsAsync(int cartDEtailsId);
    }
}