using AASC.FW.EF6;
using System.ComponentModel.DataAnnotations;

namespace AASC.Partner.API.Models
{
    public class CLADevice : Entity
    {
        //[ValidGuid]
        //public string Id { get; set; }

        [Required]
        [Display(Name = "Device")]
        public string Device { get; set; }

        [Required]
        public bool Key { get; set; }
        //public Guid? Id { get; set; }

    }
    public class CLAProduct : Entity
    {
        //[ValidGuid]
        //public string Id { get; set; }

        [Required]
        [Display(Name = "Device")]
        public string Device { get; set; }

        [Required]
        public int Amount { get; set; }
        //public Guid? Id { get; set; }

    }

}