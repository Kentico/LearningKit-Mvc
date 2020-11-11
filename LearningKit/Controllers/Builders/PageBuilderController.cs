using System.Web.Mvc;

using CMS.Core;
using CMS.DocumentEngine;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using LearningKit.Models.PageBuilder;

// Registers a route for handling requests that target pages of the 'LearningKit.PageBuilderAdvanced' type
[assembly: RegisterPageRoute("LearningKit.PageBuilderAdvanced", typeof(LearningKit.Controllers.PageBuilderController))]

namespace LearningKit.Controllers
{
    /// <summary>
    /// Handles requests for a page built using the page builder feature.
    /// Uses advanced content tree-based routing with a custom controller.
    /// </summary>
    public class PageBuilderController : Controller
    {
        private readonly IPageDataContextRetriever dataRetriever;

        public PageBuilderController(IPageDataContextRetriever dataContextRetriever)
        {
            // Initializes an instance of the page data retrieval service (available when using content tree-based routing)
            dataRetriever = dataContextRetriever;            
        }

        public ActionResult Index()
        {
            TreeNode page = dataRetriever.Retrieve<TreeNode>().Page;
            var model = new PageBuilderModel()
            {
                HeadingText = page.DocumentName
            };

            return View("PageBuilder", model);
        }
    }
}