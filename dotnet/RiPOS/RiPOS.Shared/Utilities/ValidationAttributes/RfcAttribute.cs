using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RiPOS.Shared.Utilities.ValidationAttributes
{
    public class RfcAttribute : ValidationAttribute
    {
        public bool AllowEmpty { get; set; } = true;
        public RfcAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string rfc = value?.ToString();

            if (AllowEmpty && string.IsNullOrEmpty(rfc))
                return ValidationResult.Success;

            string validRfcPattern = @"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3})?$";

            var emailRegex = new Regex(validRfcPattern, RegexOptions.IgnoreCase);

            if (emailRegex.IsMatch(rfc))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(base.ErrorMessage);
        }

    }
}
