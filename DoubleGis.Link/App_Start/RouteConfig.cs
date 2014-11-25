﻿using System.Web.Mvc;
using System.Web.Routing;

namespace DoubleGis.Link
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

			routes.MapRoute(
                name: "Search",
                url: "search/{what}/{where}",
                defaults: new { controller = "Home", action = "Search", }
            );

			routes.MapRoute(
                name: "Card",
                url: "card/{id}/{hash}",
                defaults: new { controller = "Home", action = "Card", }
            );
        }
    }
}
