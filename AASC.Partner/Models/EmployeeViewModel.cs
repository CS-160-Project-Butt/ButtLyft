using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class EmployeeViewModel
    {
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        public String JobTitle { get; set; }

        //public Guid CompanyId { get; set; }
        public string CompanyId { get; set; }

        public virtual CompanyDisplayViewModel Company { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual DisplayUserBindingModel ApplicationUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }
    }

    public class EmployeeListViewModel
    {
        public string Id { get; set; }

        public string JobTitle { get; set; }

        public string CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserFirstName { get; set; }

        public string ApplicationUserLastName { get; set; }

        public string ApplicationUserEmail { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime ActiveFrom { get; set; }

        public DateTime DeactiveFrom { get; set; }

        public string CreatedById { get; set; }

        public string CreatedByUserName { get; set; }
    }

    public class EmployeeDisplayViewModel
    {
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        public String JobTitle { get; set; }

        public string ApplicationUserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}