using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
{
    public class DisplayUserBindingModel
    {
        //[Required]
        //[Display(Name = "Id")]
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        public string Email { get; set; }

        //[Required]
        //[Display(Name = "Username")]
        public string Username { get; set; }

        //[Required]
        //[Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
