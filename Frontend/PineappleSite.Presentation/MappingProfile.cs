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
using DeleteCouponDto = PineappleSite.Presentation.Services.ShoppingCarts.DeleteCouponDto;
using DeleteProductDto = PineappleSite.Presentation.Services.Products.DeleteProductDto;
using DeleteProductsDto = PineappleSite.Presentation.Services.Products.DeleteProductsDto;
using DeleteProductViewModel = PineappleSite.Presentation.Models.Products.DeleteProductViewModel;

namespace PineappleSite.Presentation;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Coupon Mapping

        CreateMap<CouponDto, CouponViewModel>().ReverseMap();
        CreateMap<CouponDtoResult, CouponViewModel>().ReverseMap();
        CreateMap<CreateCouponDto, CreateCouponViewModel>().ReverseMap();
        CreateMap<UpdateCouponDto, UpdateCouponViewModel>().ReverseMap();
        CreateMap<Services.Coupons.DeleteCouponDto, DeleteCouponViewModel>().ReverseMap();
        CreateMap<DeleteCouponsDto, DeleteCouponsViewModel>().ReverseMap();
        CreateMap<CouponDto, ResultViewModel>().ReverseMap();

        CreateMap<CouponDtoResult, ResultViewModel>().ReverseMap();
        CreateMap<CouponDtoCollectionResult, CollectionResultViewModel<CouponViewModel>>().ReverseMap();

        #endregion

        #region Identity && User Mapping

        CreateMap<AuthRequestDto, AuthRequestViewModel>().ReverseMap();
        CreateMap<RegisterRequestDto, RegisterRequestViewModel>().ReverseMap();
        CreateMap<TokenModelDto, TokenModelViewModel>().ReverseMap();

        CreateMap<GetUserDto, GetUserViewModel>().ReverseMap();
        CreateMap<CreateUserDto, CreateUserViewModel>().ReverseMap();
        CreateMap<DeleteUserDto, DeleteUserViewModel>().ReverseMap();
        CreateMap<UpdateUserDto, UpdateUserViewModel>().ReverseMap();
        CreateMap<DeleteUsersDto, DeleteUsersViewModel>().ReverseMap();

        #endregion

        #region Product Mapping

        CreateMap<Services.Products.ProductDto, ProductViewModel>().ReverseMap();
        CreateMap<Services.Products.DeleteProductDto, Models.Products.DeleteProductViewModel>().ReverseMap();

        CreateMap<ProductDtoResult, ProductViewModel>().ReverseMap();
        CreateMap<ProductDtoResult, ProductResultViewModel>().ReverseMap();
        CreateMap<ProductDtoCollectionResult, ProductsCollectionResultViewModel<ProductViewModel>>().ReverseMap();
        CreateMap<Services.Products.DeleteProductDto, Models.Products.DeleteProductsViewModel>().ReverseMap();

        #endregion

        #region ShoppingCart Mapping

        CreateMap<Services.ShoppingCarts.CartDto, CartViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.CartHeaderDto, CartViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.CartDetailsDto, CartViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.CartHeaderDto, CartHeaderViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.CartDetailsDto, CartDetailsViewModel>().ReverseMap();
        CreateMap<DeleteProductsDto, DeleteProductsViewModel>().ReverseMap();

        CreateMap<DeleteProductDto, DeleteProductViewModel>().ReverseMap();
        CreateMap<DeleteProductsDto, DeleteProductsViewModel>().ReverseMap();
        CreateMap<DeleteProductByUserDto, DeleteProductByUserViewModel>().ReverseMap();
        CreateMap<DeleteCouponDto, DeleteCouponByCodeViewModel>().ReverseMap();
        CreateMap<DeleteCouponsByCodeDto, DeleteCouponsByCodeViewModel>().ReverseMap();

        CreateMap<DeleteProductViewModel, PineappleSite.Presentation.Services.ShoppingCarts.DeleteProductDto>().ReverseMap();
        CreateMap<DeleteProductsViewModel, PineappleSite.Presentation.Services.ShoppingCarts.DeleteProductsDto>();

        CreateMap<CartDtoResult, CartViewModel>().ReverseMap();
        CreateMap<Services.ShoppingCarts.ProductDto, ProductViewModel>().ReverseMap();

        #endregion

        #region Favourite Mapping

        CreateMap<FavouriteDto, FavouriteViewModel>().ReverseMap();
        CreateMap<FavouriteHeaderDto, FavouriteHeaderViewModel>().ReverseMap();
        CreateMap<FavouriteDetailsDto, FavouriteDetailsViewModel>().ReverseMap();
        CreateMap<DeleteProductsViewModel, DeleteFavouriteProductsDto>().ReverseMap();
        CreateMap<DeleteProductDto, DeleteProductViewModel>().ReverseMap();
        CreateMap<DeleteFavouriteProductDto, DeleteProductViewModel>().ReverseMap();

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