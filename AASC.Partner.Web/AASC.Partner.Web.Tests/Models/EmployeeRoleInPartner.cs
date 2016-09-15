using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public enum RoleInPartner
    {
        Viewer = 0,
        Contributor = 1,
        Approver = 2
    }

    public class EmployeeRoleInPartner
    {
        public string Id { get; set; }

        public RoleInPartner Role { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
