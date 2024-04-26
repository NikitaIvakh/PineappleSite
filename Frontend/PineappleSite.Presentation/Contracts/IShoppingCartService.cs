using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation.Contracts;

public interface IShoppingCartService
{
    Task<CartResult<CartViewModel>> GetCartAsync(string userId);

    Task<CartResult> CartUpsertAsync(CartViewModel cartViewModel);

    Task<CartResult<CartHeaderViewModel>> ApplyCouponAsync(CartViewModel cartViewModel);

    Task<CartResult<CartHeaderViewModel>> RemoveCouponAsync(CartViewModel cartViewModel);

    Task<CartResult> RemoveCartDetailsAsync(DeleteProductViewModel deleteProductViewModel);

    Task<CartResult> RemoveCartDetailsByUserAsync(DeleteProductByUserViewModel deleteProductByUserViewModel);

    Task<CartCollectionResult<bool>> RemoveCartDetailsAsync(DeleteProductsViewModel deleteProductListViewModel);

    Task<CartResult> RemoveCouponByCode(DeleteCouponByCodeViewModel deleteCouponByCodeViewModel);

    Task<CartResult<bool>> RabbitMqShoppingCartAsync(CartViewModel cartViewModel);
}