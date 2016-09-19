using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Helpers
{
    public class Filter
    {
        public string field { get; set; }
        public string Operator { get; set; }
        public string value { get; set; }
    }
}