using AutoMapper;
using Identity.Application.DTOs;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;

namespace Identity.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUser, CreateUserDto>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserDto>().ReverseMap();
            CreateMap<ApplicationUser, DeleteUserDto>().ReverseMap();
        }
    }
}