using AutoMapper;
using RiPOS.Shared.Enums;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles.Converters;

public class RoleEnumToRoleResponseConverter : ITypeConverter<RoleEnum, RoleResponse>
{
    public RoleResponse Convert(RoleEnum source, RoleResponse dest, ResolutionContext context) {
        return source switch
        {
            RoleEnum.SuperAdmin => new RoleResponse 
            { 
                Id = (int)RoleEnum.SuperAdmin,
                Code = "SUPERADM",
                Name = "Super Admin",
                Description = "Super Administrador"
            },
            RoleEnum.Admin => new RoleResponse 
            { 
                Id = (int)RoleEnum.Admin,
                Code = "ADM",
                Name = "Admin",
                Description = "Administrador"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }
}