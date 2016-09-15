using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class DepartmentViewModel
    {
        public string Id { get; set; }

        public String Name { get; set; }

        public string CompanyId { get; set; }

        public virtual CompanyViewModel Company { get; set; }

        public string ParentDepartmentId { get; set; }

        public virtual DepartmentDisplayViewModel ParentDepartment { get; set; }

        public string DepartmentHeadEmployeeId { get; set; }

        public virtual EmployeeViewModel DepartmentHead { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }

        public virtual ICollection<EmployeeListViewModel> Employees { get; set; }

        public virtual ICollection<DepartmentViewModel> ChildrenDepartments { get; set; }
    }

    public class DepartmentListViewModel
    {
        public string Id { get; set; }

        public String Name { get; set; }

        public string CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string ParentDepartmentId { get; set; }

        public string ParentDepartmentName { get; set; }

        public string DepartmentHeadEmployeeId { get; set; }

        public string DepartmentHeadUserName { get; set; }        

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public string CreatedByUserName { get; set; }

        public virtual ICollection<EmployeeViewModel> Employees { get; set; }

        public virtual ICollection<DepartmentListViewModel> ChildrenDepartments { get; set; }
    }

    public class DepartmentDisplayViewModel
    {
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }
    }
}