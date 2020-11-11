using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

namespace LearningKit
{
    /// <summary>
    /// Registers required implementations to the Autofac container and set the container as ASP.NET MVC dependency resolver
    /// </summary>
    public static class DependencyResolverConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            ConfigureDependencyResolverForMvcApplication(builder);

            AttachCMSDependencyResolver(builder);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }


        private static void ConfigureDependencyResolverForMvcApplication(ContainerBuilder builder)
        {
            // Enable property injection in view pages
            builder.RegisterSource(new ViewRegistrationSource());

            // Register web abstraction classes
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
        }


        /// <summary>
        /// Configures Autofac container to use CMS dependency resolver in case it cannot resolve a dependency.
        /// </summary>
        private static void AttachCMSDependencyResolver(ContainerBuilder builder)
        {
            builder.RegisterSource(new CMSRegistrationSource());
        }
    }
}