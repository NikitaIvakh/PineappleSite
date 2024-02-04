using AutoMapper;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities.Cart;

namespace ShoppingCart.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            #endregion
        }
    }
}