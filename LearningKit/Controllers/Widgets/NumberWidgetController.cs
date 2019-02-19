using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;

using LearningKit.Controllers.Widgets;
using LearningKit.Models.Widgets.NumberWidget;

// Assembly attribute to register the widget for the connected Kentico instance
[assembly: RegisterWidget("LearningKit.Widgets.NumberWidget", typeof(NumberWidgetController), "Selected number", IconClass = "icon-octothorpe")]
namespace LearningKit.Controllers.Widgets
{
    /// <summary>
    /// A sample widget displaying a message customizable by a widget property.
    /// </summary>
    public class NumberWidgetController : WidgetController<NumberWidgetProperties>
    {
        // Default GET action used to retrieve the widget markup
        public ActionResult Index()
        {
            // Retrieves the properties as a strongly typed object
            NumberWidgetProperties properties = GetProperties();

            // Creates a new model and sets its value
            var model = new NumberWidgetViewModel
            {
                Number = properties.Number
            };

            return PartialView("Widgets/_NumberWidget", model);
        }
    }
}