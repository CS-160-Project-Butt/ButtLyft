using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Helpers
{
    public class Filtering
    {
        public string logic { get; set; }
        public List<Filter> filters { get; set; }
    }
}