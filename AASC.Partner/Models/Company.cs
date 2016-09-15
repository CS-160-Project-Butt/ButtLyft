using AASC.FW.EF6;
using AASC.Partner.API.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AASC.Partner.API.Models
{
    public class Company : Entity
    {
        [ValidGuid]
        [Display(Name = "Id")]
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public virtual ICollection<Partner> Partners { get; set; }
        
    }
}