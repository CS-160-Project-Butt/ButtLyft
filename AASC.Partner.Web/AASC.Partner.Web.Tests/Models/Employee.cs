using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public class Employee
    {
        public Guid? Id { get; set; }

        public String JobTitle { get; set; }

        public Guid CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
