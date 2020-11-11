using Kentico.Forms.Web.Mvc;
using Kentico.Forms.Web.Mvc.Widgets;

namespace LearningKit.Models.Widgets.ExtendedFormWidget
{
    public class ExtendedFormWidgetProperties : FormWidgetProperties
    {
        // Defines a property and sets its default value
        // Assigns the default Xperience text input component, which allows users to enter 
        // a textual value for the property in the widget's configuration dialog
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 0, Label = "Heading text")]
        public string HeadingText { get; set; } = "Default";
    }    
}