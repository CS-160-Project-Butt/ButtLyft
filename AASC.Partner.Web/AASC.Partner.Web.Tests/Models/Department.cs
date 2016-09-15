using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public class Department
    {
        public Guid? Id { get; set; }

        public String Name { get; set; }

        public Guid CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public Guid? ParentDepartmentId { get; set; }

        public virtual Department ParentDepartment { get; set; }

        public Guid? DepartmentHeadEmployeeId { get; set; }

        public virtual Employee DepartmentHead { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
