using AutoMapper;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Models.Identities;
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

            #region
            CreateMap<AuthRequest, AuthRequestViewModel>().ReverseMap();
            CreateMap<AuthResponse, AuthResponseViewModel>().ReverseMap();
            CreateMap<RegisterRequest, RegisterRequestViewModel>().ReverseMap();
            CreateMap<RegisterResponse, RegisterResponseViewModel>().ReverseMap();
            #endregion

            CreateMap<BaseCommandResponse, ResponseViewModel>().ReverseMap();
            CreateMap<AuthResponseBaseIdentityResponse, IdentityResponseViewModel>().ReverseMap();
            CreateMap<RegisterResponseBaseIdentityResponse, IdentityResponseViewModel>().ReverseMap();
        }
    }
}