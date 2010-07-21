using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ServerInfo.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "Refresh",
                new { controller = "Home", action = "Refresh" }
            );
            routes.MapRoute(null, "Delete/{Ip}",
                new { controller = "Home", action = "Delete" }
            );
            routes.MapRoute(null, "",
                new { controller = "Home", action = "Index", SortBy = "Ip", SortDir = "Up"} // Parameter defaults
            );
            routes.MapRoute(null, "SortBy/{SortBy}",
                new { controller = "Home", action = "Index", SortDir = "Up" } // Parameter defaults
            );
            routes.MapRoute(null, "SortBy/{SortBy}/{SortDir}",
                new { controller = "Home", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, sortby = "Ip" } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}