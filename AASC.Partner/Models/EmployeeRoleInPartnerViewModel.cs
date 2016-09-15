using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class EmployeeRoleInPartnerViewModel
    {
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        public RoleInPartner Role { get; set; }

        public string EmployeeId { get; set; }
        //public Guid? EmployeeId { get; set; }

        public virtual EmployeeViewModel Employee { get; set; }

        //public Guid? PartnerId { get; set; }
        public string PartnerId { get; set; }

        public virtual PartnerViewModel Partner { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }
    }
}