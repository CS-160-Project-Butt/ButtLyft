using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AASC.Partner.API.Models
{
    public class PlatformBindingModel
    {
        public string Platform { get; set; }
    }

    public class PlatformTrimBindingModel
    {
        public string Platform { get; set; }

        public string Trim { get; set; }
    }

    public class PlatformTrimCodeNameBindingModel
    {
        public string Platform { get; set; }

        public string Trim { get; set; }

        public string CodeName { get; set; }
    }

    public class ModelLevelBindingModel
    {
        public string Level { get; set; }
    }

    public class ModelLevelCategoryBindingModel
    {
        public string Level { get; set; }

        public string Category { get; set; }
    }

    public class ModelLevelCategorySubcategoryBindingModel
    {
        public string Level { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }
    }

    public class MarketSegmentBindingModel
    {
        public string MarketSegment { get; set; }
    }
}