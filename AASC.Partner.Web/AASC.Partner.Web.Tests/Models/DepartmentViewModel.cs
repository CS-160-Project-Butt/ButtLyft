using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public class DepartmentViewModel
    {
        public Guid? Id { get; set; }

        public String Name { get; set; }

        public Guid CompanyId { get; set; }

        public virtual CompanyViewModel Company { get; set; }

        public Guid? ParentDepartmentId { get; set; }

        public virtual ParentDepartmentViewModel ParentDepartment { get; set; }

        public Guid? DepartmentHeadEmployeeId { get; set; }

        public virtual EmployeeViewModel DepartmentHead { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }

        public virtual ICollection<EmployeeViewModel> Employees { get; set; }

        public virtual ICollection<DepartmentViewModel> ChildrenDepartments { get; set; }
    }
}
