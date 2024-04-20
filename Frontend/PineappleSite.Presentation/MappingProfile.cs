using AutoMapper;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Coupons;
using PineappleSite.Presentation.Services.Favourite;
using PineappleSite.Presentation.Services.Identities;
using PineappleSite.Presentation.Services.Orders;
using PineappleSite.Presentation.Services.Products;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Coupon Mapping

        CreateMap<GetCouponDto, GetCouponViewModel>().ReverseMap();
        CreateMap<GetCouponsDto, GetCouponsViewModel>().ReverseMap();

        CreateMap<CreateCouponDto, CreateCouponViewModel>().ReverseMap();
        CreateMap<UpdateCouponDto, UpdateCouponViewModel>().ReverseMap();
        CreateMap<DeleteCouponDto, DeleteCouponViewModel>().ReverseMap();
        CreateMap<DeleteCouponsDto, DeleteCouponsViewModel>().ReverseMap();

        #endregion

        #region Identity && User Mapping

        CreateMap<AuthRequestDto, AuthRequestViewModel>().ReverseMap();
        CreateMap<AuthResponseDto, AuthResponseViewModel>().ReverseMap();
        CreateMap<RegisterRequestDto, RegisterRequestViewModel>().ReverseMap();
        CreateMap<RegisterResponseDto, RegisterResponseViewModel>().ReverseMap();
        CreateMap<TokenModelDto, TokenModelViewModel>().ReverseMap();
        CreateMap<AuthResponseDtoResult, IdentityResult>().ReverseMap();
        CreateMap<RegisterResponseDtoResult, IdentityResult>().ReverseMap();
        CreateMap<AuthResponseDtoResult, AuthResponseViewModel>().ReverseMap();
        CreateMap<RegisterResponseDtoResult, RegisterResponseViewModel>().ReverseMap();

        CreateMap<UpdateUserDto, UpdateUserViewModel>().ReverseMap();
        CreateMap<DeleteUserDto, DeleteUserViewModel>().ReverseMap();
        CreateMap<DeleteUserListDto, DeleteUserListViewModel>().ReverseMap();
        CreateMap<CreateUserDto, CreateUserViewModel>().ReverseMap();

        CreateMap<GetUserDto, GetUserViewModel>().ReverseMap();
        CreateMap<GetAllUsersDto, GetAllUsersViewModel>().ReverseMap();
        CreateMap<GetUserForUpdateDto, GetUserForUpdateViewModel>().ReverseMap();
        CreateMap<GetUserForUpdateDto, UpdateUserProfileViewModel>().ReverseMap();

        #endregion

        #region Product Mapping

        CreateMap<Services.Products.ProductDto, ProductViewModel>().ReverseMap();
        CreateMap<DeleteProductDto, DeleteProductViewModel>().ReverseMap();

        CreateMap<ProductDtoResult, ProductViewModel>().ReverseMap();
        CreateMap<ProductDtoResult, ProductResultViewModel>().ReverseMap();
        CreateMap<ProductDtoCollectionResult, ProductsCollectionResultViewModel<ProductViewModel>>().ReverseMap();
        CreateMap<DeleteProductsDto, DeleteProductsViewModel>().ReverseMap();

        #endregion

        #region ShoppingCart Mapping

        CreateMap<Services.ShoppingCarts.CartDto, CartViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.CartHeaderDto, CartHeaderViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.CartDetailsDto, CartDetailsViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.DeleteProductList, DeleteProductListViewModel>().ReverseMap();
        CreateMap<DeleteProductsViewModel, DeleteProductList>().ReverseMap();

        CreateMap<CartDtoResult, CartViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.ProductDto, ProductViewModel>().ReverseMap();

        #endregion

        #region Favourite Mapping

        CreateMap<FavouriteDto, FavouriteViewModel>().ReverseMap();
        CreateMap<FavouriteHeaderDto, FavouriteHeaderViewModel>().ReverseMap();
        CreateMap<FavouriteDetailsDto, FavouriteDetailsViewModel>().ReverseMap();
        CreateMap<DeleteFavouriteProductsDto, DeleteFavouriteProductsViewModel>().ReverseMap();
        CreateMap<DeleteProductsViewModel, DeleteFavouriteProductsViewModel>().ReverseMap();

        CreateMap<FavouriteDtoResult, FavouriteViewModel>().ReverseMap();
        CreateMap<Services.Favourite.ProductDto, ProductViewModel>().ReverseMap();

        #endregion

        #region Order Mapping

        CreateMap<Services.Orders.ProductDto, ProductViewModel>().ReverseMap();
        CreateMap<Services.Orders.CartDto, CartViewModel>().ReverseMap();
        CreateMap<Services.Orders.CartHeaderDto, CartHeaderViewModel>().ReverseMap();
        CreateMap<Services.Orders.CartDetailsDto, CartDetailsViewModel>().ReverseMap();

        CreateMap<OrderHeaderDto, OrderHeaderViewModel>().ReverseMap();
        CreateMap<OrderDetailsDto, OrderDetailsViewModel>()
            .ForMember(key => key.Product, src => src.MapFrom(key => key.Product))
            .ReverseMap();
        CreateMap<StripeRequestDto, StripeRequestViewModel>().ReverseMap();
        CreateMap<OrderHeaderDto, StripeRequestViewModel>().ReverseMap();
        CreateMap<OrderHeaderDtoResult, OrderHeaderViewModel>().ReverseMap();
        CreateMap<System.DateTimeOffset, System.DateTime>().ReverseMap();

        #endregion
    }
}