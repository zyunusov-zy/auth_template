using AuthSystemTemplate.Application.DTOs.User;
using AuthSystemTemplate.Domain.Entities;
using AutoMapper;

namespace AuthSystemTemplate.Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.UserRoles
                    .Select(ur => ur.Role.Name.ToString())
                    .ToList()));
    }
}