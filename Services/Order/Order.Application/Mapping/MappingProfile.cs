using AutoMapper;
using Order.Domain.DTOs;
using Order.Domain.Entities;

namespace Order.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderHeaderDto, CartHeaderDto>()
            .ForMember(key => key.CartTotal, cart => cart.MapFrom(src => src.OrderTotal)).ReverseMap()
            .ForMember(key => key.DeliveryDate, cart => cart.MapFrom(src => src.DeliveryDate)).ReverseMap();

        CreateMap<CartDetailsDto, OrderDetailsDto>()
            .ForMember(key => key.ProductName, order => order.MapFrom(src => src.Product.Name))
            .ForMember(key => key.Price, order => order.MapFrom(src => src.Product.Price));
        CreateMap<OrderDetailsDto, CartDetailsDto>();

        CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
        CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
    }
}