using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AASC.Partner.API.Configuration.Intel
{
    public class IntelRoadmapConfig
    {
        static string datasource = "Platforms.xml";
        static string datasourceLevels = "Levels.xml";
        static string datasourceMarketSegments = "MarketSegments.xml";
        static string datasourceRoadmapStatus = "RoadmapStatus.xml";

        public static List<string> GetMarketSegments()
        {
            var root = "~/Configuration/Intel/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<string>();
            var xml = XElement.Load(string.Format("{0}{1}", path, datasourceMarketSegments));
            //var data = new List<string>();
            //var xml = XElement.Load(datasourceMarketSegments);
            var segments = from d in xml.Elements("MarketSegment")
                           select d;
            foreach (XElement p in segments)
            {
                data.Add(p.Attribute("name").Value);
            }

            return data;
        }

        public static List<String> GetPlatforms()
        {
            var root = "~/Configuration/Intel/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<string>();
            var xml = XElement.Load(string.Format("{0}{1}", path, datasource));
            var platforms = from d in xml.Elements("Platform")
                            select d;
            foreach (XElement p in platforms)
            {
                data.Add(p.Attribute("name").Value);
            }
            return data;
        }

        public static List<string> GetTrims(string platform)
        {
            var root = "~/Configuration/Intel/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<string>();
            var xml = XElement.Load(string.Format("{0}{1}", path, datasource));
            //var data = new List<string>();
            //var xml = XElement.Load(datasource);
            var platforms = (from d in xml.Elements("Platform")
                             where d.Attribute("name").Value == platform
                             select d).ToList();
            foreach (XElement p in platforms)
            {
                foreach (var el in p.Elements()) // trims
                {
                    foreach (var e in el.Elements()) // trim
                    {
                        data.Add(e.Attribute("name").Value);
                        //data.Add(e.Value);
                    }
                }
            }
            return data;
        }

        public static List<string> GetCodeNames(string platform, string trim)
        {
            var root = "~/Configuration/Intel/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<string>();
            var xml = XElement.Load(string.Format("{0}{1}", path, datasource));
            //var data = new List<string>();
            //var xml = XElement.Load(datasourceLevels);
            var platforms = (from d in xml.Elements("Platform")
                             where d.Attribute("name").Value == platform
                             select d).ToList();
            foreach (XElement p in platforms)
            {
                Console.WriteLine(p.Attribute("name").Value);

                foreach (var el in p.Elements()) // platforms
                {
                    foreach (var e in el.Elements().Where(x => x.Attribute("name").Value == trim)) // trim
                    {
                        foreach (var s in e.Elements())
                        {
                            foreach (var sub in s.Elements())
                            {
                                data.Add(sub.Value);
                            }
                        }
                    }
                }
            }
            return data;
        }

        public static List<string> GetLevels()
        {
            var root = "~/Configuration/Intel/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<string>();
            var xml = XElement.Load(string.Format("{0}{1}", path, datasourceLevels));
            //var data = new List<string>();
            //var xml = XElement.Load(datasourceLevels);
            var levels = from d in xml.Elements("Level")
                         select d;
            foreach (XElement p in levels)
            {
                data.Add(p.Attribute("name").Value);
            }
            return data;
        }

        public static List<string> GetCategories(string level)
        {
            var root = "~/Configuration/Intel/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<string>();
            var xml = XElement.Load(string.Format("{0}{1}", path, datasourceLevels));
            //var data = new List<string>();
            //var xml = XElement.Load(datasourceLevels);
            var levels = from d in xml.Elements("Level")
                         where d.Attribute("name").Value == level
                         select d;
            foreach (XElement p in levels)
            {
                Console.WriteLine(p.Attribute("name").Value);

                foreach (var el in p.Elements()) // categories
                {
                    foreach (var e in el.Elements()) // category
                    {
                        data.Add(e.Attribute("name").Value);
                    }
                }
            }
            return data;
        }

        public static List<string> GetSubcategories(string level, string category)
        {
            var root = "~/Configuration/Intel/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<string>();
            var xml = XElement.Load(string.Format("{0}{1}", path, datasourceLevels));
            //var data = new List<string>();
            //var xml = XElement.Load(datasourceLevels);
            var levels = (from d in xml.Elements("Level")
                          where d.Attribute("name").Value == level
                          select d).ToList();
            foreach (XElement p in levels)
            {
                Console.WriteLine(p.Attribute("name").Value);

                foreach (var el in p.Elements()) // categories
                {
                    foreach (var e in el.Elements().Where(x => x.Attribute("name").Value == category)) // category
                    {
                        foreach (var s in e.Elements())
                        {
                            foreach (var sub in s.Elements())
                            {
                                data.Add(sub.Value);
                            }
                        }
                    }
                }
            }
            return data;
        }

        public static List<String> GetRoadmapStatus()
        {
            var root = "~/Configuration/Intel/";
            var path = HttpContext.Current.Server.MapPath(root);
            var data = new List<string>();
            var xml = XElement.Load(string.Format("{0}{1}", path, datasourceRoadmapStatus));
            //var data = new List<string>();
            //var xml = XElement.Load(datasourceRoadmapStatus);
            var platforms = from d in xml.Elements("Status")
                            select d;
            foreach (XElement p in platforms)
            {
                data.Add(p.Value);
            }
            return data;
        }
    }
}