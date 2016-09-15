using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public class Company
    {
        public Guid? Id { get; set; }

        public String Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public virtual ICollection<Partner> Partners { get; set; }
    }
}
