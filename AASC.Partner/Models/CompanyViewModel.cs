using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class CompanyViewModel
    {
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        public String Name { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }

        public virtual ICollection<DepartmentViewModel> Departments { get; set; }

        public virtual ICollection<Partner> Partners { get; set; }
    }

    public class CompanyListViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public string CreatedByUserName { get; set; }

        public virtual ICollection<DepartmentListViewModel> Departments { get; set; }

        public virtual ICollection<PartnerListViewModel> Partners { get; set; }
    }


    public class CompanyDisplayViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

    }
}