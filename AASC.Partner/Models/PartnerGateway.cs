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
    public class PartnerGateway
    {
        [ValidGuid]
        [Display(Name = "Id")]
        public string Id { get; set; }
        //public Guid? Id { get; set; }

        [Required]
        [DefaultValue(TransactionType.BuySell)]
        [Display(Name = "Gateway")]
        public TransactionType TransactionType { get; set; }

        [Display(Name = "Gateway")]
        [MaxLength(256)]
        public string Gateway { get; set; }

        [Display(Name = "Gateway User Id")]
        [MaxLength(100)]
        public string GatewayUserId { get; set; }

        [Display(Name = "Gateway Password")]
        [MaxLength(100)]
        public string GatewayPassword { get; set; }

        [Display(Name = "Test Inbound Folder")]
        [MaxLength(100)]
        public string Inbound { get; set; }

        [Display(Name = "Outbound Folder")]
        [MaxLength(100)]
        public string Outbound { get; set; }

        [Display(Name = "Test Gateway")]
        [MaxLength(256)]
        public string TestGateway { get; set; }

        [Display(Name = "Test Gateway User Id")]
        [MaxLength(100)]
        public string TestGatewayUserId { get; set; }

        [Display(Name = "Test Gateway Password")]
        [MaxLength(100)]
        public string TestGatewayPassword { get; set; }

        [Display(Name = "Test Inbound Folder")]
        [MaxLength(100)]
        public string TestInbound { get; set; }

        [Display(Name = "Test Outbound Folder")]
        [MaxLength(100)]
        public string TestOutbound { get; set; }

        [Required]
        [Display(Name = "Partner Id")]
        public string PartnerId { get; set; }
        //public Guid PartnerId { get; set; }

        [Required]
        [ForeignKey("PartnerId")]
        public virtual Partner Partner { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Expire Date")]
        public DateTime ExpireDate { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; }
    }
}