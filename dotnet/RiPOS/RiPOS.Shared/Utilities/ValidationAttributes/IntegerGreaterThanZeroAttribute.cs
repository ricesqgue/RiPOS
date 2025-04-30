using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Utilities.ValidationAttributes;

public class IntegerGreaterThanZeroAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null && int.TryParse(value.ToString(), out int number))
        {
            if (number > 0)
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(ErrorMessage);
    }
}