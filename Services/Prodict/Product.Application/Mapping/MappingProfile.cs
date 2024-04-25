using AutoMapper;
using Product.Domain.DTOs;
using Product.Domain.Entities.Producrs;

namespace Product.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductEntity, ProductDto>().ReverseMap();
        CreateMap<ProductEntity, CreateProductDto>().ReverseMap();
        CreateMap<ProductEntity, UpdateProductDto>().ReverseMap();
        CreateMap<ProductEntity, DeleteProductDto>().ReverseMap();
        CreateMap<ProductEntity, DeleteProductsDto>().ReverseMap();
    }
}