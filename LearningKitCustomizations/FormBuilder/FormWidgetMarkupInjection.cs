using System;
using System.Web.Mvc;

using Kentico.Forms.Web.Mvc.Widgets;


namespace FormBuilderCustomizations
{
    public class FormWidgetMarkupInjection
    {

        public static void RegisterEventHandlers()
        {
            FormWidgetRenderingConfiguration.GetConfiguration.Execute += FormWidgetInjectMarkup;
        }


        private static void FormWidgetInjectMarkup(object sender, GetFormWidgetRenderingConfigurationEventArgs e)
        {
            e.Configuration.FormWrapperConfiguration = new FormWrapperRenderingConfiguration
            {
                // Renders the form's display name above the form using the 'CustomHtmlEnvelopeString' property
                // FormWrapperRenderingConfiguration.CONTENT_PLACEHOLDER acts as a placeholder for the form's body in the resulting markup
                CustomHtmlEnvelopeString = $@"<h1>{e.Form.FormDisplayName}</h1> {FormWrapperRenderingConfiguration.CONTENT_PLACEHOLDER}"
            };

            // Sets additional attributes only for specific forms. Since the 'class' attribute is fairly
            // common, checks if the key is already present and inserts or appends the key accordingly.
            if (e.Form.FormName.Equals("ContactUs", StringComparison.InvariantCultureIgnoreCase))
            {
                if (e.Configuration.SubmitButtonHtmlAttributes.ContainsKey("class"))
                {
                    e.Configuration.SubmitButtonHtmlAttributes["class"] += " contactus-submit-button";
                }
                else
                {
                    e.Configuration.SubmitButtonHtmlAttributes["class"] = "contactus-submit-button";
                }
            }
        }
    }
}