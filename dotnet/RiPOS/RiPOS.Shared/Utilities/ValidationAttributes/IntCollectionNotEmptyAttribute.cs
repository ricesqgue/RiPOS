using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Utilities.ValidationAttributes;

public class IntCollectionNotEmptyAttribute : ValidationAttribute
{
    public bool AllowZero { get; set; } = true;


    public IntCollectionNotEmptyAttribute()
    {
        ErrorMessage = "The {0} field must be a non-empty collection.";
    }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult($"The {validationContext.DisplayName} field is required and must contain at least one element.");
        }
        
        if (value is not IEnumerable<int> collection)
        {
            return new ValidationResult($"The {validationContext.DisplayName} field must be a collection of integers.");
        }

        var enumerable = collection.ToList();
        if (!enumerable.Any())
        {
            return new ValidationResult($"The {validationContext.DisplayName} field must contain at least one element.");
        }

        if (!AllowZero)
        {
            if (enumerable.Any(item => item == 0))
            {

                string message = ErrorMessageString != ErrorMessage
                    ? string.Format(ErrorMessageString, validationContext.DisplayName)
                    : $"The {validationContext.DisplayName} field cannot contain zero.";
                return new ValidationResult(message);
            }
        }
        
        return ValidationResult.Success;
    }
}