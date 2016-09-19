using AASC.FW.EF6;
using AASC.FW.Infrastructure;
using AASC.Partner.API.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AASC.Partner.API.Models
{
    public class Employee : Entity
    {
        [ValidGuid]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public String JobTitle { get; set; }

        [Required]
        [ValidGuid]
        [Display(Name = "Company Id")]
        public string CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [Required]
        [Display(Name = "Application User Id")]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ActiveFrom { get; set; }

        [Required]
        public DateTime DeactiveFrom { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; }
    }
}