using AASC.FW.EF6;
using AASC.Partner.API.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public enum TransactionType
    {
        Purchase,
        BuySell
    }

    public enum RecursiveCycle
    {
        Daily = 0,
        Weekly = 1,
        Biweekly = 2,
        Monthly = 3,
        Bimonthly = 4,
        Quartly = 5,
        Semiannually = 6,
        Annually = 7
    }

    public class PartnerAgreement : Entity, IValidatableObject
    {
        [ValidGuid]
        [Display(Name = "Id")]
        public string Id { get; set; }
        //public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Type of Agreement")]
        public TransactionType TransactionType { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MaxLength(256)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Expire Date")]
        public DateTime ExpireDate { get; set; }

        [Required]
        [Display(Name = "Invoice Cycle")]
        public RecursiveCycle InvoiceCycle { get; set; }
        
        [DefaultValue(0)]
        [Display(Name = "Default Invoice Day")]
        public int InvoiceDayOn { get; set; }

        [Display(Name = "Document Id")]
        public string FileUploadId { get; set; }
        //public Guid? FileUploadId { get; set; }

        [Display(Name = "Document")]
        public virtual FileUpload Document { get; set; }

        [Required]
        [Display(Name = "Partner Id")]
        public string PartnerId { get; set; }
        //public Guid PartnerId { get; set; }

        [Required]
        [ForeignKey("PartnerId")]
        public virtual Partner Partner { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (InvoiceCycle == RecursiveCycle.Monthly || InvoiceCycle == RecursiveCycle.Bimonthly ||
                InvoiceCycle == RecursiveCycle.Quartly || InvoiceCycle == RecursiveCycle.Semiannually ||
                InvoiceCycle == RecursiveCycle.Annually)
            {
                if (InvoiceDayOn <= 0 || InvoiceDayOn > 31)
                {
                    yield return new ValidationResult(
                        string.Format("InvoiceDayOn must be between 1 to 31"),
                        new[] { "InvoiceDayOn" });
                }                
            }
            if (InvoiceCycle == RecursiveCycle.Weekly || InvoiceCycle == RecursiveCycle.Biweekly)
            {
                if (InvoiceDayOn > 5 || InvoiceDayOn < 1)
                {
                    yield return new ValidationResult(
                        string.Format("InvoiceDayOn must be between 1 to 5 whenever you choose weekly or biweekly billing cycle which means Monday to Friday"),
                        new[] { "InvoiceDayOn" });
                }
            }
            if (InvoiceCycle == RecursiveCycle.Daily && InvoiceDayOn != 0)
            {
                yield return new ValidationResult(
                    "InvoiceDayOn must be 0 whenever you choose invoice cycle as daily.",
                    new string[] { "InvoiceDayOn" });
            }
        }
    }
}