using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserStoreRoles.Select(usr => usr.Role)));
    }
}