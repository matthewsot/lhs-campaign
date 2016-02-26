﻿using System.Web.Mvc;
using System.Web.Routing;

namespace LHSCamp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{graduationYear}",
                defaults: new { controller = "Candidates", action = "GetClass", graduationYear = UrlParameter.Optional }
            );
        }
    }
}
