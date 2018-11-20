using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestExcel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ImportExcel", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    "TimeSchedule",
            //    "TimeSchedule/Index/{BR_NAME}/{BR_Semester}/{BR_Year}",                            
            //    new { controller = "TimeSchedule", action = "Index" ,BR_NAME = UrlParameter.Optional , BR_Semester = UrlParameter.Optional , BR_Year  = UrlParameter.Optional }  
            //);
        }
    }
}
