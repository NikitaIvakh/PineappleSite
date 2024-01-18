using AutoMapper;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Coupons;
using PineappleSite.Presentation.Services.Identities;

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

            CreateMap<UserWithRolesDto, UserWithRolesViewModel>().ForMember(key => key.Roles, opt => opt.MapFrom(key => key.Roles)).ReverseMap();
            CreateMap<UserWithRolesDto, UpdateUserProfileViewModel>().ForMember(key => key.Roles, opt => opt.MapFrom(key => key.Roles)).ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();

            CreateMap<UpdateUserDto, UpdateUserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserViewModel>().ReverseMap();
            CreateMap<DeleteUserDto, DeleteUserViewModel>().ReverseMap();

            CreateMap<UpdateUserProfileDto, UpdateUserProfileViewModel>().ReverseMap();
            #endregion

            CreateMap<BaseCommandResponse, ResponseViewModel>().ReverseMap();
        }
    }
}