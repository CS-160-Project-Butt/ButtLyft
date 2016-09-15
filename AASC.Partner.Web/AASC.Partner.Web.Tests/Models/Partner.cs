using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public class Partner
    {
        public Guid? Id { get; set; }

        public String Name { get; set; }

        public String LandingPage { get; set; }

        public String Logo { get; set; }

        public String Theme { get; set; }

        public String Roles { get; set; }

        public String Address { get; set; }

        public String City { get; set; }

        public String ZipCode { get; set; }

        public String Region { get; set; }

        public String Country { get; set; }

        public bool IsActive { get; set; }

        public DateTime RegisterDate { get; set; }

        public virtual ICollection<ApplicationUser> UserAccounts { get; set; }

        public virtual ICollection<PartnerAgreement> PartmentAgreements { get; set; }

        public virtual ICollection<PartnerGateway> PartnerGateways { get; set; }

        public virtual ICollection<EmployeeRoleInPartner> EmployeeRoleInPartners { get; set; }
    }
}
