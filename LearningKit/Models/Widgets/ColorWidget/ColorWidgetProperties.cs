using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using LearningKit.FormBuilder.FormComponents;

namespace LearningKit.Models.Widgets.ColorWidget
{
    public class ColorWidgetProperties : IWidgetProperties
    {
        [EditingComponent(ColorFormComponent.IDENTIFIER, Order = 0, Label = "Color")]
        public string Color { get; set; } = "white";
    }
}