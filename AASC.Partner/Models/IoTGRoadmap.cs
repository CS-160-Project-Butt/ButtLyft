using AASC.FW.EF6;
using AASC.Partner.API.Configuration.Intel;
using AASC.Partner.API.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class IoTGRoadmap : Entity, IValidatableObject
    {
        [ValidGuid]
        public string Id { get; set; }
        //public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }

        //[Required]
        [Display(Name = "Status")]
        [IoTGRoadmapStatusValidationAttribute(ErrorMessage = "The value is not allowed.")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Platform")]
        [IoTGPlatformValidationAttribute(ErrorMessage = "The value is not allowed.")]
        public string Platform { get; set; }

        [Required]
        [Display(Name = "Trim")]
        public string Trim { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Code Name")]
        public string CodeName { get; set; }

        [Required]
        [Display(Name = "Level")]
        [IoTGLevelValidationAttribute(ErrorMessage = "The value is not allowed.")]
        public string Level { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Subcategory")]
        public string Subcategory { get; set; }

        [Display(Name = "Data Sheet")]
        public string Link { get; set; }

        [Display(Name = "Data Sheet")]
        public string FileUploadId { get; set; }
        //public Guid? FileUploadId { get; set; }

        [Display(Name = "Document")]
        public virtual FileUpload Document { get; set; }

        [Display(Name = "Market Segment")]
        [IoTGMarketSegmentValidationAttribute(ErrorMessage = "The value is not allowed.")]
        public string MarketSegment { get; set; }

        [Display(Name = "Avail. ES-Sample")]
        public string AvailabilityESSample { get; set; }

        [Display(Name = "Avail. MP")]
        public string AvailabilityMP { get; set; }

        //[Required]
        [Display(Name = "Contact Employee Id")]
        public string ContactEmployeeId { get; set; }
        //public Guid ContactEmployeeId { get; set; }

        //[Required]
        [ForeignKey("ContactEmployeeId")]
        public Employee Contact { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {            
            string[] availableTrims = IntelRoadmapConfig.GetTrims(Platform).ToArray();
            if (!availableTrims.Contains(Trim))
            {
                yield return new ValidationResult(
                    string.Format("Trim must be subset of {{0}}", string.Join(",", availableTrims)),
                    new[] { "Trim" });
            }
            string[] availableCategories = IntelRoadmapConfig.GetCategories(Level).ToArray();
            if (!availableCategories.Contains(Category))
            {
                yield return new ValidationResult(
                    string.Format("Category must be subset of {{0}}", string.Join(",", availableCategories)),
                    new[] { "Category" });
            }
            string[] availableSubcategories = IntelRoadmapConfig.GetSubcategories(Level, Category).ToArray();
            if (availableSubcategories.Length > 0)
            {
                if (!availableSubcategories.Contains(Subcategory))
                {
                    yield return new ValidationResult(
                        string.Format("Subcategory must be subset of {{0}}", string.Join(",", availableSubcategories)),
                        new[] { "Subcategory" });
                }
            }
        }
    }
}