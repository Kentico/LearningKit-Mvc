using CMS.DataEngine;

using Kentico.Forms.Web.Mvc;


namespace LearningKit.Models.FormBuilder.CustomFormComponents
{
    //DocSection:PropertiesDefinition
    public class RgbInputComponentProperties : FormComponentProperties<string>
    {
        // Sets the component as the editing component of its DefaultValue property
        // System properties of the specified editing component, such as the Label, Tooltip, and Order, remain set to system defaults unless explicitly set in the constructor
        [DefaultValueEditingComponent("RgbInputComponent", DefaultValue = "#ff0000")]
        public override string DefaultValue
        {
            get;
            set;
        }


        // Initializes a new instance of the RgbInputComponentProperties class and configures the underlying database field
        public RgbInputComponentProperties()
            : base(FieldDataType.Text, 7)
        {
        }
    }
    //EndDocSection:PropertiesDefinition
}