using AutoMapper;
using Identity.Application.DTOs.Identities;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUser>().ReverseMap();
            CreateMap<UserWithRoles, UserWithRolesDto>().ReverseMap();
            CreateMap<ApplicationUser, CreateUserDto>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserDto>().ReverseMap();
            CreateMap<ApplicationUser, DeleteUserDto>().ReverseMap();
        }
    }
}