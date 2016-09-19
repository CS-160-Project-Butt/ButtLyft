using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class PartnerViewModel
    {
        //public Guid? Id { get; set; }
        public string Id { get; set; }

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

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }

        public virtual ICollection<DisplayUserBindingModel> UserAccounts { get; set; }

        public virtual ICollection<PartnerAgreementViewModel> PartmentAgreements { get; set; }

        public virtual ICollection<PartnerGatewayViewModel> PartnerGateways { get; set; }

        public virtual ICollection<EmployeeRoleInPartner> EmployeeRoleInPartners { get; set; }
    }

    public class PartnerListViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LandingPage { get; set; }

        public string Logo { get; set; }

        public string Theme { get; set; }

        public string Roles { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public string CreatedByUserName { get; set; }

        public virtual ICollection<DisplayUserBindingModel> UserAccounts { get; set; }

        public virtual ICollection<PartnerAgreementViewModel> PartmentAgreements { get; set; }

        public virtual ICollection<PartnerGatewayViewModel> PartnerGateways { get; set; }

        public virtual ICollection<EmployeeRoleInPartner> EmployeeRoleInPartners { get; set; }
    }

}