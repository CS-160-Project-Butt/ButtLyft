using AASC.FW.EF6;
using AASC.Partner.API.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AASC.Partner.API.Models
{
    public enum RoleInPartner
    {
        Viewer = 0,
        Contributor = 1,
        Approver = 2
    }

    public class EmployeeRoleInPartner : Entity
    {
        [ValidGuid]
        [Display(Name = "Id")]
        public string Id { get; set; }
        //public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Role")]
        public RoleInPartner Role { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string EmployeeId { get; set; }
        //public Guid EmployeeId { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string PartnerId { get; set; }
        //public Guid PartnerId { get; set; }

        [Required]
        [ForeignKey("PartnerId")]
        public virtual Partner Partner { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; }

    }
}