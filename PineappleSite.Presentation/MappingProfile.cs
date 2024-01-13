using AutoMapper;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

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
            #endregion
        }
    }
}