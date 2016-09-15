using System;
using System.Collections.Generic;

namespace AASC.Partner.API.Models
{
    public class PhaseOutPrepViewModel
    {
        public string Id { get; set; }

        public bool Phased { get; set; }
        
        public string PartNumber { get; set; }
        
        public string Description { get; set; }
        
        public string PlmStatus { get; set; }
        
        public string ProductFamily { get; set; }
        
        public string LastBuyTime { get; set; }
        
        public string Replacement { get; set; }

        public string CreatedBy { get; set; }
        
        public string CreatedDate { get; set; }
    }

}