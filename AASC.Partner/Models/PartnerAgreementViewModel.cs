using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class PartnerAgreementViewModel
    {
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public RecursiveCycle InvoiceCycle { get; set; }

        public int InvoiceDayOn { get; set; }

        public string FileUploadId { get; set; }
        //public Guid? FileUploadId { get; set; }

        public virtual FileUploadBindingModel Document { get; set; }

        public string PartnerId { get; set; }
        //public Guid? PartnerId { get; set; }

        public virtual PartnerViewModel Partner { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }
    }
}