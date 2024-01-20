using AutoMapper;
using Product.Application.DTOs.Products;
using Product.Core.Entities.Producrs;

namespace Product.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region
            CreateMap<ProductEntity, ProductDto>()
                .ForMember(key => key.ProductCategory, opt => opt.MapFrom(key => key.ProductCategory))
                .ReverseMap();
            #endregion
        }
    }
}