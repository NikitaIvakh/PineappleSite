using AutoMapper;
using Favourites.Domain.DTOs;
using Favourites.Domain.Entities.Favourite;

namespace Favourites.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FavouritesHeader, FavoutiteHeaderDto>().ReverseMap();
            CreateMap<FavouritesDetails, FavouritesDetailsDto>().ReverseMap();
            CreateMap<ProductDto, ProductDto>().ReverseMap();
        }
    }
}