using RiPOS.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Utilities.ValidationAttributes;

public class RolesInEnumAttribute : ValidationAttribute
{
    public RolesInEnumAttribute()
    {

    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            var roles = (List<int>?)value;

            if (roles == null || roles.Count == 0)
            {
                return new ValidationResult("Los roles son requeridos");
            }

            foreach (var role in roles)
            {
                if (!Enum.IsDefined(typeof(RoleEnum), role))
                {
                    return new ValidationResult($"El rol \"{role}\" no es válido");
                }
            }

            return ValidationResult.Success;
        }
        catch (Exception)
        {
            return new ValidationResult("Los roles son requeridos");
        }
    }
}