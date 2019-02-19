using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;

using LearningKit.Controllers.Sections;

[assembly: RegisterSection("LearningKit.Sections.DefaultSection", typeof(DefaultSectionController), "Default section", IconClass = "icon-square")]
namespace LearningKit.Controllers.Sections
{
    //DocSection:DefaultSection
    public class DefaultSectionController : Controller
    {
        // GET action used to retrieve the section markup
        public ActionResult Index()
        {
            return PartialView("Sections/_DefaultSection");
        }
    }
    //EndDocSection:DefaultSection
}