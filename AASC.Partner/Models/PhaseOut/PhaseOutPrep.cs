using AASC.FW.EF6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AASC.Partner.API.Models
{
    public class PhaseOutPrep : Entity
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Phased")]
        public bool Phased { get; set; }

        [Required]
        [Display(Name = "Part Number")]
        public string PartNumber { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Plm Status")]
        public string PlmStatus { get; set; }

        [Required]
        [Display(Name = "Product Family")]
        public string ProductFamily { get; set; }

        [Required]
        [Display(Name = "Last Buy Time")]
        public string LastBuyTime { get; set; }

        [Required]
        [Display(Name = "Replacement")]
        public string Replacement { get; set; }
        
        [Required]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }


        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

    }
}