using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AASC.Partner.API.Configuration.Cla
{
    public class CLAConfig
    {
        static string deviceDataSource = "CLADeviceCategory.xml";
        static string productDataSource = "CLAProducts.xml";
        static string salesRepsDataSource = "SalesReps.xml";
        static string claStatusDataSource = "CLAStatus.xml";


        public static List<object> GetStatuses()
        {
            var root = "~/Configuration/Cla/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<object>();
            var xml = XElement.Load(string.Format("{0}{1}", path, claStatusDataSource));

            var statuses = from d in xml.Elements("Status")
                          select d;

            foreach (XElement d in statuses)
            {
                data.Add(new { Status = d.Attribute("name").Value});
            }
            return data;
        }

        public static List<object> GetDevices()
        {
            var root = "~/Configuration/Cla/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<object>();
            var xml = XElement.Load(string.Format("{0}{1}", path, deviceDataSource));

            var devices = from d in xml.Elements("Device")
                          select d;

            foreach (XElement d in devices)
            {
                data.Add(new { Device = d.Attribute("name").Value, Selected = false });
            }
            return data;
        }


        public static List<object> GetProductTypes()
        {
            var root = "~/Configuration/Cla/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<object>();
            var xml = XElement.Load(string.Format("{0}{1}", path, productDataSource));

            var types = from d in xml.Elements("Type")
                        select d;

            foreach (XElement d in types)
            {
                data.Add(new { ProductType = d.Attribute("name").Value });
            }
            return data;
        }

        public static List<object> GetProducts()
        {
            var root = "~/Configuration/Cla/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<object>();
            var xml = XElement.Load(string.Format("{0}{1}", path, productDataSource));

            var type = (from d in xml.Elements("Type")
                        select d).ToList();

            foreach (XElement d in type)
            {
                //typeList.Add(d.Attribute("name").Value);

                foreach (XElement c in d.Elements())
                {
                    data.Add(new { ProductType = d.Attribute("name").Value, Product = c.Attribute("name").Value, Quantity = 0 });
                    //data.Add(c.Attribute("name").Value);
                }

            }
            return data;
        }

        public static List<object> GetSalesReps()
        {
            var root = "~/Configuration/Cla/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<object>();
            var xml = XElement.Load(string.Format("{0}{1}", path, salesRepsDataSource));

            var reps = from r in xml.Elements("Rep")
                       select r;

            foreach (XElement d in reps)
            {
                data.Add(new { name = d.Attribute("name").Value, role = d.Attribute("role").Value });
            }
            return data;
        }

        public static string getEmail(string salesrep)
        {

            var root = "~/Configuration/Cla/";
            var path = HttpContext.Current.Server.MapPath(root);
            var xml = XElement.Load(string.Format("{0}{1}", path, salesRepsDataSource));

            var reps = from r in xml.Elements("Rep")
                       select r;

            foreach (XElement d in reps)
            {
                if (salesrep.ToLower() == d.Attribute("name").Value.ToLower())
                {
                    return d.Attribute("email").Value;
                }
            }
            return null;
        }

        public static string getPMEmail()
        {//assumes only one pm exists
            var root = "~/Configuration/Cla/";
            var path = HttpContext.Current.Server.MapPath(root);
            var xml = XElement.Load(string.Format("{0}{1}", path, salesRepsDataSource));

            var reps = from r in xml.Elements("Rep")
                       select r;

            foreach (XElement d in reps)
            {
                if ("pm" == d.Attribute("role").Value.ToLower())
                {
                    return d.Attribute("email").Value;
                }
            }
            return null;
        }



    }

}