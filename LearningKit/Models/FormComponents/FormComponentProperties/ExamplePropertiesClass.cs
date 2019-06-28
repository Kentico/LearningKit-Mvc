using System.ComponentModel.DataAnnotations;

using CMS.DataEngine;

using Kentico.Forms.Web.Mvc;


namespace LearningKit.FormBuilder.FormComponentProperties
{
    public class ExamplePropertiesClass : FormComponentProperties<string>
    {
        //DocSection:DefaultValueEditingAttribute
        // Sets a custom editing component for the DefaultValue property
        // System properties of the specified editing component, such as the Label, Tooltip, and Order, remain set to system defaults unless explicitly set in the constructor
        [DefaultValueEditingComponent(TextInputComponent.IDENTIFIER)]
        public override string DefaultValue
        {
            get;
            set;
        } = "DefaultValue";
        //EndDocSection:DefaultValueEditingAttribute


        //DocSection:ConfigureSystemProperties
        // Assigns the 'IntInputComponent' as the editing component of the 'MyProperty' property and configures its system properties
        [EditingComponent(IntInputComponent.IDENTIFIER, ExplanationText = "Please enter a number", Label = "Custom property", Tooltip = "Stores a numeric value.", Order = 0)]
        [Required]
        public int MyProperty { get; set; }
        //EndDocSection:ConfigureSystemProperties


        //DocSection:SystemPropertiesOverride
        // Overrides the Label property and sets its editing component to 'TextInputComponent'
        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Label")]
        public override string Label { get; set; }


        // Overrides the Tooltip property and sets its editing component to 'TextInputComponent'
        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Tooltip")]
        public override string Tooltip { get; set; }
        //EndDocSection:SystemPropertiesOverride


        //DocSection:CustomPropertyRegistration
        // Assigns the 'TextInputComponent' as the editing component of the 'CustomProperty' property
        [EditingComponent(TextInputComponent.IDENTIFIER)]
        public string CustomProperty { get; set; }
        //EndDocSection:CustomPropertyRegistration


        //DocSection:CustomPropertyConfiguration
        // Enables the 'ACustomProperty' property for editing via the properties panel and assigns 'CustomFormComponent' as its editing component
        [EditingComponent("CustomFormComponent")]
        // Sets the editing component's 'MyProperty' property to '10'
        [EditingComponentProperty("MyProperty", 10)]
        // Sets the editing component's 'CustomProperty' to a localized value using the 'customproperty.value' resource string
        [EditingComponentProperty("CustomProperty", "{$customproperty.value$}")]
        public string ACustomProperty { get; set; }
        //EndDocSection:CustomPropertyConfiguration


        //DocSection:DataSourceConfiguration
        // Assigns a selector component to the SelectedOption property
        [EditingComponent(DropDownComponent.IDENTIFIER)]
        // Configures the list options available in the selector
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), "cz;Czech Republic\r\nusa;United States")]
        public string SelectedOption { get; set; }
        //EndDocSection:DataSourceConfiguration


        // Initializes a new instance of the ExamplePropertiesClass class and configures the underlying database field
        public ExamplePropertiesClass()
            : base(FieldDataType.Unknown)
        {
        }
    }
}