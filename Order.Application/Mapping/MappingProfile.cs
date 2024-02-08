﻿using AutoMapper;
using Order.Domain.DTOs;
using Order.Domain.Entities;

namespace Order.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();

            CreateMap<OrderDetailsDto, CartDetailsDto>().ReverseMap();
            CreateMap<OrderHeaderDto, CartHeaderDto>().ReverseMap();
            CreateMap<OrderHeaderDto, List<OrderHeaderDto>>().ReverseMap();

            CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(key => key.CartTotal, cart => cart.MapFrom(src => src.OrderTotal)).ReverseMap();

            CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(key => key.ProductName, order => order.MapFrom(src => src.Product.Name))
                .ForMember(key => key.Price, order => order.MapFrom(src => src.Product.Price))
                .ReverseMap();
        }
    }
}