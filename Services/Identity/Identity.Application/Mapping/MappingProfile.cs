using AutoMapper;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;

namespace Identity.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, CreateUserDto>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserDto>().ReverseMap();
            CreateMap<ApplicationUser, DeleteUserDto>().ReverseMap();
            CreateMap<UserWithRoles, UserWithRolesDto>().ReverseMap();

            CreateMap<ApplicationUser, GetAllUsersDto>().ReverseMap();
            CreateMap<ApplicationUser, GetUserForUpdateDto>().ReverseMap();
            CreateMap<ApplicationUser, GetUserDto>().ReverseMap();
        }
    }
}