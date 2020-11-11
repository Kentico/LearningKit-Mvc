using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Kentico.Web.Mvc;
using Kentico.OnlineMarketing.Web.Mvc;

namespace LearningKit
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Enables and configures the selected Xperience integration features for ASP.NET MVC 
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            // Mapping of routes must be done after registration of Xperience MVC features (using the ApplicationBuilder instance)
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Registers the dependency resolver for the application
            DependencyResolverConfig.Register();
        }


        //DocSection:ApplicationError
        protected void Application_Error()
        {
            // Sets 404 HTTP exceptions to be handled via IIS (behavior is specified in the "httpErrors" section in the Web.config file)
            var error = Server.GetLastError();
            if ((error as HttpException)?.GetHttpCode() == 404)
            {
                Server.ClearError();
                Response.StatusCode = 404;
            }
        }
        //EndDocSection:ApplicationError


        //DocSection:GetVaryByCustom
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            // Creates the options object used to store individual cache key parts
            IOutputCacheKeyOptions options = OutputCacheKeyHelper.CreateOptions();

            // Selects a caching configuration according to the current custom string
            switch (custom)
            {
                case "Default":
                    // Sets the variables that compose the cache key for the 'Default' VaryByCustom string
                    options
                        .VaryByHost()
                        .VaryByBrowser()
                        .VaryByUser();
                    break;

                case "OnlineMarketing":
                    // Sets the variables that compose the cache key for the 'OnlineMarketing' VaryByCustom string
                    options
                        .VaryByCookieLevel()
                        .VaryByPersona()
                        .VaryByABTestVariant();
                    break;
            }

            // Combines individual 'VaryBy' key parts into a cache key under which the output is cached
            string cacheKey = OutputCacheKeyHelper.GetVaryByCustomString(context, custom, options);

            // Returns the constructed cache key
            if (!String.IsNullOrEmpty(cacheKey))
            {
                return cacheKey;
            }

            // Calls the base implementation if the provided custom string does not match any predefined configurations
            return base.GetVaryByCustomString(context, custom);
        }
        //EndDocSection:GetVaryByCustom
    }
}
