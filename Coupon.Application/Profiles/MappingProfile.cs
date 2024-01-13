using AutoMapper;
using Coupon.Application.DTOs;
using Coupon.Core.Entities;

namespace Coupon.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CouponEntity, CouponDto>().ReverseMap();
            CreateMap<CouponEntity, CreateCouponDto>().ReverseMap();
            CreateMap<CouponEntity, UpdateCouponDto>().ReverseMap();
            CreateMap<CouponEntity, DeleteCouponListDto>().ReverseMap();
        }
    }
}