using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using CMS.ContactManagement;

using Kentico.Forms.Web.Mvc;
using Kentico.Web.Mvc;

namespace LearningKit
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Enables and configures the selected Kentico ASP.NET MVC integration features
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            // Mapping of routes must be done after registration of Kentico MVC features (using the ApplicationBuilder instance)
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            // Sets the rendering configuration for the 'Form' page builder widget
            SetWidgetFormFieldRenderingConfuration();
        }


        //DocSection:WidgetConfiguration
        // Configures the rendering configuration for the 'Form' page builder widget
        private void SetWidgetFormFieldRenderingConfuration()
        {
            FormFieldRenderingConfiguration.Widget.ColonAfterLabel = false;
            FormFieldRenderingConfiguration.Widget.ComponentWrapperConfiguration = new ElementRenderingConfiguration
            {
                ElementName = "div",
                HtmlAttributes = new Dictionary<string, object> { { "class", "formFieldComponent" } }
            };
        }
        //EndDocSection:WidgetConfiguration


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
        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            // Defines caching requirements based on the on-line marketing data of the current contact
            if (arg == "contactdata")
            {
                // Gets the current contact, without creating a new anonymous contact for new visitors
                ContactInfo currentContact = ContactManagementContext.GetCurrentContact(createAnonymous: false);

                if (currentContact != null)
                {
                    // Ensures separate caching for each combination of the following contact variables: contact groups, persona, gender
                    // Note: This does NOT define separate caching for every contact
                    return String.Format("ContactData={0}|{1}|{2}",
                        String.Join("|", currentContact.ContactGroups.Select(c => c.ContactGroupName).OrderBy(x => x)),
                        currentContact.ContactPersonaID,
                        currentContact.ContactGender);
                }
            }
            
            return base.GetVaryByCustomString(context, arg);
        }
        //EndDocSection:GetVaryByCustom
    }
}
