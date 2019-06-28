using CMS.DataEngine;

using Kentico.Forms.Web.Mvc;

namespace LearningKit.FormBuilder.FormComponentProperties
{
    public class CustomFormComponentProperties : FormComponentProperties<string>
    {
        //DocSection:PropertiesDefinition
        // Sets a custom editing component for the DefaultValue property
        // System properties of the specified editing component, such as the Label, Tooltip, and Order, remain set to system defaults unless explicitly set in the constructor
        [DefaultValueEditingComponent(TextInputComponent.IDENTIFIER)]
        public override string DefaultValue
        {
            get;
            set;
        }


        // Initializes a new instance of the CustomFormComponentProperties class and configures the underlying database field
        public CustomFormComponentProperties()
            : base(FieldDataType.Text, size: 200)
        {
        }
        //EndDocSection:PropertiesDefinition
    }
}
