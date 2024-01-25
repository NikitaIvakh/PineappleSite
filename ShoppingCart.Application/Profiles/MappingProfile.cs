using AutoMapper;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Core.Entities.Cart;

namespace ShoppingCart.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            CreateMap<Core.Entities.DTOs.ProductDto, ProductDto>().ReverseMap();
            #endregion
        }
    }
}