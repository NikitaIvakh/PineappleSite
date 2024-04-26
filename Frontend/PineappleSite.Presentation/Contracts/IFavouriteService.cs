﻿using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Favourite;

namespace PineappleSite.Presentation.Contracts;

public interface IFavouriteService
{
    Task<FavouriteResult<FavouriteViewModel>> GetFavouriteProductsAsync(string userId);

    Task<FavouriteResult<FavouriteHeaderViewModel>> FavouriteProductUpsertAsync(FavouriteViewModel favouriteViewModel);

    Task<FavouriteResult> DeleteFavouriteProductAsync(int productId);

    Task<FavouriteCollectionResult> DeleteFavouriteProductsAsync(DeleteProductsViewModel deleteProductsViewModel);
}