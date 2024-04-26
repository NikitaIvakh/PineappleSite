using AutoMapper;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
        CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
    }
}