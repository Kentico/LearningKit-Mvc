using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

namespace LearningKit.Models.PageTemplates.LandingPage
{
    public class LandingPageProperties : IPageTemplateProperties
    {
       // Defines a property and sets its default value
       // Assigns the default Xperience checkbox component, which saves a boolean value 
       // depending on the state of the checkbox in the page template's configuration dialog
       [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 0, Label = "Show title")]
       public bool ShowTitle { get; set; } = true;
    }
}