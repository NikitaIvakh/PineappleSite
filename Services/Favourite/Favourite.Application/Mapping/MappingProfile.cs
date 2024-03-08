using AutoMapper;
using Favourite.Domain.DTOs;
using Favourite.Domain.Entities;

namespace Favourite.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FavouriteHeader, FavouriteHeaderDto>().ReverseMap();
            CreateMap<FavouriteDetails, FavouriteDetailsDto>().ReverseMap();
        }
    }
}