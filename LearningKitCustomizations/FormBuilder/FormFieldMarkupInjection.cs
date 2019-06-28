using System;

using Kentico.Forms.Web.Mvc;


namespace FormBuilderCustomizations
{
    public class FormFieldMarkupInjection
    {
        public static void RegisterEventHandlers()
        {
            // Contextually customizes the markup of rendered form fields
            FormFieldRenderingConfiguration.GetConfiguration.Execute += InjectMarkupIntoKenticoComponents;
        }


        private static void InjectMarkupIntoKenticoComponents(object sender, GetFormFieldRenderingConfigurationEventArgs e)
        {
            // Only injects additional markup into default Kentico form components
            if (!e.FormComponent.Definition.Identifier.StartsWith("Kentico", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            // Adds WAI-ARIA and HTML5 accessibility attributes to form fields marked as 'Required' via the Form builder interface
            AddAccessibilityAttributes(e);

            // Assigns additional attributes to 'TextArea' fields
            AddFieldSpecificMarkup(e);

            // Assigns additional attributes to fields rendered as part of a specified form
            AddFormSpecificMarkup(e);
        }


        private static void AddAccessibilityAttributes(GetFormFieldRenderingConfigurationEventArgs e)
        {
            if (e.FormComponent.BaseProperties.Required)
            {
                // Adds the 'aria-required' and 'required' attributes to the component's 'input' element
                e.Configuration.EditorHtmlAttributes["aria-required"] = "true";
                e.Configuration.EditorHtmlAttributes["required"] = "";

                // Appends an asterisk to the component's 'Label' element. Since the class attribute is fairly
                // common, checks if the key is already present and inserts or appends the key accordingly.
                if (e.Configuration.LabelHtmlAttributes.ContainsKey("class"))
                {
                    e.Configuration.LabelHtmlAttributes["class"] += " required-field-red-star";
                }
                else
                {
                    e.Configuration.LabelHtmlAttributes["class"] = "required-field-red-star";
                }
            }
        }


        private static void AddFieldSpecificMarkup(GetFormFieldRenderingConfigurationEventArgs e)
        {
            // Sets a custom CSS class for the wrapping element of 'TextAreaComponent' type form fields
            if (e.FormComponent is TextAreaComponent)
            {
                if (e.Configuration.RootConfiguration.HtmlAttributes.ContainsKey("class"))
                {
                    e.Configuration.RootConfiguration.HtmlAttributes["class"] += " text-area-styles";
                }
                else
                {
                    e.Configuration.RootConfiguration.HtmlAttributes["class"] = "text-area-styles";
                }
            }
        }


        private static void AddFormSpecificMarkup(GetFormFieldRenderingConfigurationEventArgs e)
        {
            // Gets the context of the Form for which the field is being rendered
            BizFormComponentContext context = e.FormComponent.GetBizFormComponentContext();

            // Modifies only form fields rendered as part of the 'ContactUs' form
            if (context.FormInfo.FormName.Equals("ContactUs", StringComparison.InvariantCultureIgnoreCase))
            {
                e.Configuration.ColonAfterLabel = false;
                e.Configuration.EditorHtmlAttributes["data-formname"] = context.FormInfo.FormDisplayName;
            }
        }
    }
}