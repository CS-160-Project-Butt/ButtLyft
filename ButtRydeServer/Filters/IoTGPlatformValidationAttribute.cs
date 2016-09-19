using AASC.Partner.API.Configuration.Intel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IoTGPlatformValidationAttribute : ValidationAttribute
    {        
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var platforms = IntelRoadmapConfig.GetPlatforms();
            if (platforms.Contains((string)value))
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessageString);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IoTGLevelValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var levels = IntelRoadmapConfig.GetLevels();
            if (levels.Contains((string)value))
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessageString);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IoTGMarketSegmentValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return ValidationResult.Success;
            var segments = IntelRoadmapConfig.GetMarketSegments();
            if (segments.Contains((string)value))
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessageString);
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IoTGRoadmapStatusValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return ValidationResult.Success;
            var status = IntelRoadmapConfig.GetRoadmapStatus();
            if (status.Contains((string)value))
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessageString);
        }
    }
}