using AASC.FW.EF6;
using AASC.Partner.API.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AASC.Partner.API.Models
{    
    public class Partner : Entity, IValidatableObject
    {
        [ValidGuid]
        [Display(Name = "Id")]
        public string Id { get; set; }
        //public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public String Name { get; set; }

        [Display(Name = "Landing Page")]
        [MaxLength]
        public String LandingPage { get; set; }

        [Display(Name = "Logo")]
        [MaxLength]
        public String Logo { get; set; }

        [Display(Name = "Theme")]
        [MaxLength]
        public String Theme { get; set; }

        [Required]
        [Display(Name = "Roles")]
        [MaxLength(100)]
        public String Roles { get; set; }

        [Display(Name = "Address")]
        [MaxLength(256)]
        public String Address { get; set; }

        [Display(Name = "City")]
        [MaxLength(100)]
        public String City { get; set; }

        [Display(Name = "Zip Code")]
        [MaxLength(50)]
        public String ZipCode { get; set; }

        [Display(Name = "Region")]
        [MaxLength(50)]
        public String Region { get; set; }

        [Display(Name = "Country")]
        [MaxLength(50)]
        public String Country { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        
        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; }

        public virtual ICollection<ApplicationUser> UserAccounts { get; set; }

        public virtual ICollection<PartnerAgreement> PartmentAgreements { get; set; }

        public virtual ICollection<PartnerGateway> PartnerGateways { get; set; }

        public virtual ICollection<EmployeeRoleInPartner> EmployeeRoleInPartners { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string[] availableRoles = { "Customer", "Manufacturer", "Vendor" };
            string[] allRoles = Array.ConvertAll(availableRoles, p => p.Trim().ToUpper());
            string[] rolesAssign = Array.ConvertAll(Roles.Split(','), p => p.ToUpper().Trim()).Where(p=>!string.IsNullOrEmpty(p)).ToArray();
            
            if (!rolesAssign.Except(allRoles).Any())
            {
                yield return new ValidationResult(
                    string.Format("Roles must be subset of {{0}}", string.Join(",", availableRoles)),
                    new[] { "Roles" });
            }
        }
    }
}