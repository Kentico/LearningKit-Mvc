//DocSection:Using
using System;
using System.Web.Mvc;

using CMS.Core;
//EndDocSection:Using

namespace LearningKit.Controllers
{
    //DocSection:AdminRedirectController
    public class AdminRedirectController : Controller
    {
        private readonly IAppSettingsService appSettingsService;

        public AdminRedirectController(IAppSettingsService appSettingsService)
        {
            this.appSettingsService = appSettingsService;
        }

        // GET: Redirects to the administration interface URL of the connected Xperience application
        public ActionResult Index()
        {
            // Loads the administration interface URL from the 'CustomAdminUrl' appSettings key in the web.config
            string adminUrl = appSettingsService["CustomAdminUrl"];

            if (!String.IsNullOrEmpty(adminUrl))
            {
                // Redirects to the specified administration interface URL 
                return RedirectPermanent(adminUrl);
            }

            // If the 'CustomAdminUrl' web.config key is not set, returns a 404 Not Found response 
            return HttpNotFound();
        }
    }
    //EndDocSection:AdminRedirectController
}