using System;
using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;

using LearningKit.Controllers.Widgets;

// Assembly attribute to register the widget for the connected Xperience instance.
[assembly: RegisterWidget("LearningKit.Widgets.RandomWidget", typeof(RandomWidgetController), "Random number", IconClass = "icon-modal-question")]
namespace LearningKit.Controllers.Widgets
{
    /// <summary>
    /// A sample widget displaying a message with a random number.
    /// </summary>
    public class RandomWidgetController : WidgetController
    {
        private static Random rnd = new Random();

        // Default GET action used to retrieve the widget markup
        public ActionResult Index()
        {
            return PartialView("Widgets/_RandomWidget", rnd.Next(100));
        }
    }
}