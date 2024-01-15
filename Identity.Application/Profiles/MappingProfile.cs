using AutoMapper;
using Identity.Application.DTOs.Identity;
using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;

namespace Identity.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuthRequest, AuthRequestDto>().ReverseMap();
            CreateMap<RegisterRequest, RegisterRequestDto>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
        }
    }
}