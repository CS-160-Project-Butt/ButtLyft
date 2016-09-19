using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class PartnerGatewayViewModel
    {
        //public Guid? Id { get; set; }
        public string Id { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Gateway { get; set; }

        public string GatewayUserId { get; set; }

        public string GatewayPassword { get; set; }

        public string Inbound { get; set; }

        public string Outbound { get; set; }

        public string TestGateway { get; set; }

        public string TestGatewayUserId { get; set; }

        public string TestGatewayPassword { get; set; }

        public string TestInbound { get; set; }

        public string TestOutbound { get; set; }

        //public Guid? PartnerId { get; set; }
        public string PartnerId { get; set; }

        public virtual PartnerViewModel Partner { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedById { get; set; }

        public virtual DisplayUserBindingModel CreatedBy { get; set; }
    }
}