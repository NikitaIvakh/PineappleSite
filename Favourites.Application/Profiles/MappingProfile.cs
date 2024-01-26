using AutoMapper;
using Favourites.Application.DTOs;
using Favourites.Domain.Entities.Favourite;

namespace Favourites.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FavouritesHeader, FavoutiteHeaderDto>().ReverseMap();
            CreateMap<FavouritesDetails, FavouritesDetailsDto>().ReverseMap();
            CreateMap<Domain.Entities.DTOs.ProductDto, ProductDto>().ReverseMap();
        }
    }
}