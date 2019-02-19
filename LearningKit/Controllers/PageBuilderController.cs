using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace LearningKit.Controllers
{
    public class PageBuilderController : Controller
    {
        public ActionResult Index()
        {
            //DocSection:Initialize
            // Retrieves the page from the Kentico database
            TreeNode page = DocumentHelper.GetDocuments()
                .Path("/Home")
                .OnCurrentSite()
                .TopN(1)
                .FirstOrDefault();

            // Returns a 404 error when the retrieving is unsuccessful
            if (page == null)
            {
                return HttpNotFound();
            }

            // Initializes the page builder with the DocumentID of the page
            HttpContext.Kentico().PageBuilder().Initialize(page.DocumentID);
            //EndDocSection:Initialize

            return View("PageBuilder");
        }
    }
}