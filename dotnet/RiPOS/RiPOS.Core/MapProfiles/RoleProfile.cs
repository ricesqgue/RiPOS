using AutoMapper;
using RiPOS.Core.MapProfiles.Converters;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleEnum, RoleResponse>()
            .ConvertUsing<RoleEnumToRoleResponseConverter>();
    }
}