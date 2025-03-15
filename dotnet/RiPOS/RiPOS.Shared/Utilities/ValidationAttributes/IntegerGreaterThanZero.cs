﻿using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Utilities.ValidationAttributes
{
    public class IntegerGreaterThanZero : ValidationAttribute
    {
        public IntegerGreaterThanZero()
        {
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (int.TryParse(value.ToString(), out int number))
            {
                if (number > 0)
                {
                    return ValidationResult.Success;
                }

            }

            return new ValidationResult(base.ErrorMessage);
        }
    }
}
