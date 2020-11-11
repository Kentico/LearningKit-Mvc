using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace LearningKit
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Maps routes for Xperience HTTP handlers and enabled MVC features (as registered in ApplicationConfig.cs)
            // Must be registered first, since some Xperience URLs may otherwise match the default ASP.NET MVC route,
            // which would result in content being displayed incorrectly
            routes.Kentico().MapRoutes();

            //DocSection:AdminRedirectRoute
            // Redirects to the connected Xperience administration interface if the URL path is '/admin'
            routes.MapRoute(
                name: "Admin",
                url: "admin",
                defaults: new { controller = "AdminRedirect", action = "Index" }
            );
            //EndDocSection:AdminRedirectRoute

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
