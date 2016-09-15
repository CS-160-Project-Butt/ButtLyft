using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASC.Partner.Web.Tests.Models
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

    public class PartnerAgreement
    {
        public Guid? Id { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public RecursiveCycle InvoiceCycle { get; set; }

        public int InvoiceDayOn { get; set; }

        public Guid? FileUploadId { get; set; }

        public virtual FileUpload Document { get; set; }

        public Guid PartnerId { get; set; }

        public virtual Partner Partner { get; set; }
    }
}
