using CMS;
using CMS.DataEngine;

using FormBuilderCustomizations;

// Registers the custom module to Kentico. The assembly's 'AssemblyInfo.cs' file must include the 'AssemblyDiscoverable' assembly attribute.
[assembly: RegisterModule(typeof(FormBuilderCustomizationsModule))]

namespace FormBuilderCustomizations
{
    public class FormBuilderCustomizationsModule : Module
    {
        public const string MODULE_NAME = "FormBuilderCustomizationsModule";


        // Module class constructor, inherits from the base constructor with the code name of the module as the parameter
        public FormBuilderCustomizationsModule()
            : base(MODULE_NAME)
        {
        }


        // Initializes the module. Called when the application starts.
        protected override void OnInit()
        {
            base.OnInit();

            // Sets global rendering configurations for forms built using the Form builder 
            // and registers event handlers that contextually modify form markup
            FormBuilderStaticMarkupConfiguration.SetGlobalRenderingConfigurations();
            FormFieldMarkupInjection.RegisterEventHandlers();
            FormWidgetMarkupInjection.RegisterEventHandlers();
        }
    }
}