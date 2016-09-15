using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public class EmployeeViewModel
    {
        public Guid? Id { get; set; }

        public String JobTitle { get; set; }

        public Guid CompanyId { get; set; }

        public virtual CompanyDisplayViewModel Company { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual DisplayUserBindingModel ApplicationUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }
    }

    public class EmployeeDisplayViewModel
    {
        public Guid? Id { get; set; }

        public String JobTitle { get; set; }

        public string ApplicationUserId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
