using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;

using LearningKit.Controllers.Sections;

// Registers the '50/50' page builder section
// This section does not require a custom controller, but uses one for demonstration purposes
[assembly: RegisterSection("LearningKit.Sections.Col5050", typeof(Col5050Controller), "50/50 columns", IconClass = "icon-l-cols-2")]

namespace LearningKit.Controllers.Sections
{
    public class Col5050Controller : SectionController
    {
        // GET action used to retrieve the section markup
        public ActionResult Index()
        {
            return PartialView("Sections/_Col5050");
        }
    }
}