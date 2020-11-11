using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace LearningKit.Models.Sections.CustomSection
{
    public class CustomSectionProperties : ISectionProperties
    {
        // Defines a property and sets its default value
        // Assigns the default Xperience text input component, which allows users to enter
        // a string value for the property in the section's configuration dialog
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 0, Label = "Color")]
        public string Color { get; set; } = "#FFF";

    }
}