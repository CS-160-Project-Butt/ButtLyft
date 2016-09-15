using System.Web;
using System.Web.Mvc;

namespace AASC.Partner.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // uncomment below line for ssl
            // filters.Add(new RequireHttpsAttribute());
        }
    }
}
