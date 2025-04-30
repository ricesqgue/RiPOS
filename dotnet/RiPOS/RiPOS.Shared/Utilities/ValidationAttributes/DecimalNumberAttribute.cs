using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RiPOS.Shared.Utilities.ValidationAttributes;

public class DecimalNumberAttribute : ValidationAttribute
{
    public bool AllowZero { get; set; } = false;

    public bool AllowNegative { get; set; } = false;
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Null values are considered valid by this attribute. Use [Required] for null checks.
        if (value == null)
        {
            return ValidationResult.Success;
        }

        // Try converting the value to decimal
        if (!decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numberValue))
        {
            ErrorMessage = "El {0} debe ser un número válido.";
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        // Check for negative numbers if not allowed
        if (!AllowNegative && numberValue < 0)
        {
            ErrorMessage = "El {0} debe ser un número no negativo.";
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        // Check for zero if not allowed
        if (!AllowZero && numberValue == 0)
        {
            ErrorMessage = "El {0} debe ser un número distinto de cero.";
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        // All checks passed
        return ValidationResult.Success;
    }
}