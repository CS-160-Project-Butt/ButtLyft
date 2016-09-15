using AASC.FW.EF6;
using AASC.Partner.API.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AASC.Partner.API.Models
{
    public class Department : Entity
    {
        [ValidGuid]
        [Display(Name = "Id")]
        public string Id { get; set; }
        //public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Company Id")]
        public string CompanyId { get; set; }
        //public Guid CompanyId { get; set; }

        [Required]
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        //public Guid? ParentDepartmentId { get; set; }
        public string ParentDepartmentId { get; set; }

        [ForeignKey("ParentDepartmentId")]
        public virtual Department ParentDepartment { get; set; }

        [Display(Name = "Department Head Employee Id")]
        public string DepartmentHeadEmployeeId { get; set; }
        //public Guid? DepartmentHeadEmployeeId { get; set; }

        [ForeignKey("DepartmentHeadEmployeeId")]
        public virtual Employee DepartmentHead { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Department> ChildrenDepartments { get; set; }
    }
}