using AutoMapper;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Coupons;
using PineappleSite.Presentation.Services.Identities;
using PineappleSite.Presentation.Services.Products;
using PineappleSite.Presentation.Services.ShoppingCarts;

namespace PineappleSite.Presentation
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Coupon Mapping
            CreateMap<CouponDto, CouponViewModel>().ReverseMap();
            CreateMap<CreateCouponDto, CreateCouponViewModel>().ReverseMap();
            CreateMap<UpdateCouponDto, UpdateCouponViewModel>().ReverseMap();
            CreateMap<DeleteCouponDto, DeleteCouponViewModel>().ReverseMap();
            CreateMap<DeleteCouponListDto, DeleteCouponListViewModel>().ReverseMap();
            #endregion

            #region Identity && User Mapping
            CreateMap<AuthRequestDto, AuthRequestViewModel>().ReverseMap();
            CreateMap<AuthResponseDto, AuthResponseViewModel>().ReverseMap();
            CreateMap<RegisterRequestDto, RegisterRequestViewModel>().ReverseMap();
            CreateMap<RegisterResponseDto, RegisterResponseViewModel>().ReverseMap();
            CreateMap<AuthResponseDtoBaseIdentityResponse, IdentityResponseViewModel>().ReverseMap();
            CreateMap<RegisterResponseDtoBaseIdentityResponse, IdentityResponseViewModel>().ReverseMap();

            CreateMap<UserWithRolesDto, UpdateUserProfileViewModel>().ReverseMap();
            CreateMap<UserWithRolesDto, UserWithRolesViewModel>()
                .ForPath(key => key.User.Id, opt => opt.MapFrom(key => key.User.Id))
                .ForPath(key => key.User.FirstName, opt => opt.MapFrom(key => key.User.FirstName))
                .ForPath(key => key.User.LastName, opt => opt.MapFrom(key => key.User.LastName))
                .ForPath(key => key.User.UserName, opt => opt.MapFrom(key => key.User.UserName))
                .ForPath(key => key.User.Email, opt => opt.MapFrom(key => key.User.Email))
                .ForPath(key => key.User.Age, opt => opt.MapFrom(key => key.User.Age))
                .ForPath(key => key.User.Description, opt => opt.MapFrom(key => key.User.Description))
                .ForPath(key => key.User.ImageUrl, opt => opt.MapFrom(key => key.User.ImageUrl))
                .ForPath(key => key.User.ImageLocalPath, opt => opt.MapFrom(key => key.User.ImageLocalPath))
                .ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();

            CreateMap<UpdateUserDto, UpdateUserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserViewModel>().ReverseMap();
            CreateMap<DeleteUserDto, DeleteUserViewModel>().ReverseMap();
            CreateMap<DeleteUserListDto, DeleteUserListViewModel>().ReverseMap();

            //CreateMap<UpdateUserProfileDto, UpdateUserProfileViewModel>().ReverseMap();
            #endregion

            #region Product Mapping
            CreateMap<Services.Products.ProductDto, ProductViewModel>().ReverseMap();
            CreateMap<DeleteProductDto, DeleteProductViewModel>().ReverseMap();
            CreateMap<DeleteProductsDto, DeleteProductsViewModel>().ReverseMap();
            #endregion

            #region ShoppingCart Mapping
            CreateMap<Services.ShoppingCarts.ProductDto, ProductViewModel>().ReverseMap();
            CreateMap<CartHeaderDto, CartHeaderViewModel>().ReverseMap();
            CreateMap<CartDetailsDto, CartDetailsViewModel>().ReverseMap();
            CreateMap<CartDto, CartViewModel>().ReverseMap();
            #endregion

            #region
            CreateMap<CartHeaderDto, CartHeaderViewModel>().ReverseMap();
            CreateMap<CartDetailsDto, CartDetailsViewModel>().ReverseMap();
            CreateMap<CartDto, CartViewModel>().ReverseMap();
            #endregion

            CreateMap<BaseCommandResponse, ResponseViewModel>().ReverseMap();
            CreateMap<ProductAPIResponse, ProductAPIViewModel>().ReverseMap();
            CreateMap<ShoppingCartAPIResponse, ShoppingCartResponseViewModel>().ReverseMap();
        }
    }
}