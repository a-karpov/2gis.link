using System.Web.Mvc;
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
                name: "Geolocation",
                url: "@geolocation",
                defaults: new { controller = "Home", action = "Geolocation" }
            );

			routes.MapRoute(
                name: "Recognize",
                url: "{query}",
                defaults: new { controller = "Home", action = "Recognize" }
            );

			routes.MapRoute(
                name: "Search",
                url: "{what}/{where}/{page}",
                defaults: new { controller = "Home", action = "Search", page = UrlParameter.Optional  }
            );


        }
    }
}
