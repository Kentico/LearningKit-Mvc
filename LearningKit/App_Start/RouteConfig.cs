using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;


using Kentico.Web.Mvc;

namespace LearningKit
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Maps routes for Kentico HTTP handlers and enabled MVC features (as registered in ApplicationConfig.cs)
            // Must be registered first, since some Kentico URLs may otherwise match the default ASP.NET MVC route,
            // which would result in content being displayed incorrectly
            routes.Kentico().MapRoutes();

            //DocSection:AdminRedirectRoute
            // Redirects to the connected Kentico administration interface if the URL path is '/admin'
            routes.MapRoute(
                name: "Admin",
                url: "admin",
                defaults: new { controller = "AdminRedirect", action = "Index" }
            );
            //EndDocSection:AdminRedirectRoute

            //DocSection:ListingRoute
            routes.MapRoute(
                name: "Store",
                url: "Store/{controller}",
                defaults: new { action = "Listing" },
                constraints: new { controller = "LearningProductType" }
            );
            //EndDocSection:ListingRoute

            //DocSection:ProductRoute
            routes.MapRoute(
                name: "Product",
                url: "Product/{guid}/{productAlias}",
                defaults: new { controller = "Product", action = "Detail" },
                constraints: new { guid = new GuidRouteConstraint() }
            );
            //EndDocSection:ProductRoute

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
