using AutoMapper;
using Identity.Application.DTOs.Identity;
using Identity.Core.Entities.Identities;

namespace Identity.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuthRequest, AuthRequestDto>().ReverseMap();
            CreateMap<RegisterRequest, RegisterRequestDto>().ReverseMap();
        }
    }
}