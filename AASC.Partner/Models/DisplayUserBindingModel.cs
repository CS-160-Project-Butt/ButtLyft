using System;
using System.ComponentModel.DataAnnotations;

namespace AASC.Partner.API.Models
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
        //[Display(Name = "UserName")]
        public string UserName { get; set; }

        //[Required]
        //[Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}