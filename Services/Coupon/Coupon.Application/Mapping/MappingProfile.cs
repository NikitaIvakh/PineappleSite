using AutoMapper;
using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;

namespace Coupon.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CouponEntity, GetCouponDto>().ReverseMap();
            CreateMap<CouponEntity, GetCouponsDto>().ReverseMap();
            
            CreateMap<CouponEntity, CreateCouponDto>().ReverseMap();
            CreateMap<CouponEntity, UpdateCouponDto>().ReverseMap();
            CreateMap<CouponEntity, DeleteCouponDto>().ReverseMap();
            CreateMap<CouponEntity, DeleteCouponsDto>().ReverseMap();
        }
    }
}