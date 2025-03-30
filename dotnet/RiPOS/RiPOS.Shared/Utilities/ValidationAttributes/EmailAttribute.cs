using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RiPOS.Shared.Utilities.ValidationAttributes
{
    public class EmailAttribute : ValidationAttribute
    {
        public bool AllowEmpty { get; set; } = true;

        public EmailAttribute()
        {

        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? email = value?.ToString();

            if (AllowEmpty && string.IsNullOrEmpty(email))
                return ValidationResult.Success;

            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var emailRegex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

            if (emailRegex.IsMatch(email!))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
