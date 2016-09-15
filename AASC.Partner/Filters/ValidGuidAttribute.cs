using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Filters
{
    public class ValidGuidAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' does not contain a valid guid";

        public ValidGuidAttribute() : base(DefaultErrorMessage) { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = Convert.ToString(value, CultureInfo.CurrentCulture);

            if (string.IsNullOrWhiteSpace(input))
                return null;

            Guid guid;
            if (!Guid.TryParse(input, out guid))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else
                return ValidationResult.Success;
        }
    }
}