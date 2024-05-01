using AutoMapper;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;

namespace Identity.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, GetUserDto>().ReverseMap();
        CreateMap<ApplicationUser, GetUsersDto>().ReverseMap();
        CreateMap<ApplicationUser, GetUserForUpdateDto>().ReverseMap();
        CreateMap<ApplicationUser, GetUsersProfileDto>().ReverseMap();

        CreateMap<ApplicationUser, CreateUserDto>().ReverseMap();
        CreateMap<ApplicationUser, UpdateUserDto>().ReverseMap();
        CreateMap<ApplicationUser, DeleteUserDto>().ReverseMap();
    }
}