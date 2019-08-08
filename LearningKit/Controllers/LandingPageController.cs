using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine;

using Kentico.PageBuilder.Web.Mvc.PageTemplates;

namespace LearningKit.Controllers
{
    public class LandingPageController : Controller
    {
        //DocSection:PageTemplateAction
        /// <summary>
        /// A GET action displaying the page where you wish to use page templates.
        /// </summary>
        /// <param name="pageAlias">Page alias of the displayed page.</param>
        public ActionResult Index(string pageAlias)
        {            
            // Retrieves the page from the Kentico database
            TreeNode page = DocumentHelper.GetDocuments()
                .Path("/Landing-pages", PathTypeEnum.Children)
                .WhereEquals("NodeAlias", pageAlias)
                .OnCurrentSite()
                .TopN(1)
                .FirstOrDefault();

            // Returns a 404 error when the retrieving is unsuccessful
            if (page == null)
            {
                return HttpNotFound();
            }

            // Returns a TemplateResult object, created with an identifier of the page as its parameter
            // Automatically initializes the page builder feature for any editable areas placed within templates
            return new TemplateResult(page.DocumentID);
        }
        //EndDocSection:PageTemplateAction
    }
}