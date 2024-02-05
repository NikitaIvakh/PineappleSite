using AutoMapper;
using Favourites.Domain.DTOs;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region ShoppingCart Mapping
            CreateMap<CartHeader, CartHeader>();
            CreateMap<CartDetails, CartDetailsDto>();
            #endregion
        }
    }
}