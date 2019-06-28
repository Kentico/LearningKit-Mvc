using CMS.DataEngine;

using Kentico.Forms.Web.Mvc;

namespace LearningKit.FormBuilder.FormComponentProperties
{
    //DocSection:PlaceholderTextProp
    public class CharacterSizeProperties : FormComponentProperties<string>
    {
        // Gets or sets the default value of the form component and the underlying field
        [DefaultValueEditingComponent(TextInputComponent.IDENTIFIER)]
        public override string DefaultValue
        {
            get;
            set;
        }


        // Defines a custom property and its editing component
        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "Size", DefaultValue = 40, Tooltip = "Enter the number of characters the field's width should be set to.", ExplanationText = "Sets the field's width to exactly fit the specified number of characters", Order = -95)]
        public int CharacterSize { get; set; }


        // Initializes a new instance of the properties class and configures the underlying database field
        public CharacterSizeProperties()
            : base(FieldDataType.Text, 500)
        {
        }
    }
    //EndDocSection:PlaceholderTextProp
}