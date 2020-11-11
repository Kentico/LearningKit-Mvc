using System.Web.Mvc;

using LearningKit.Controllers.FormSections;

//DocSection:FormSectionRegistration
using Kentico.Forms.Web.Mvc;

[assembly: RegisterFormSection("LearningKit.FormSections.TwoColumns", typeof(TwoColumnFormSectionController), "Two columns", Description = "Organizes fields into two equal-width columns side-by-side.", IconClass = "icon-l-cols-2")]
//EndDocSection:FormSectionRegistration

namespace LearningKit.Controllers.FormSections
{
    //DocSection:FormSectionController
    public class TwoColumnFormSectionController : Controller
    {
        // Action used to retrieve the section markup
        public ActionResult Index()
        {
            return PartialView("FormSections/_TwoColumnFormSection");
        }
    }
    //EndDocSection:FormSectionController
}