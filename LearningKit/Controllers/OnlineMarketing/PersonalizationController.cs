using System.Web.Mvc;

using CMS.ContactManagement;

using LearningKit.Models.Personalization;


namespace LearningKit.Controllers
{
    public class PersonalizationController : Controller
    {
        /// <summary>
        /// Gets the current contact, if contact tracking is enabled for the connected Xperience instance.
        /// </summary>
        private ContactInfo CurrentContact => ContactManagementContext.GetCurrentContact();


        /// <summary>
        /// Displays a page with a personalized greeting.
        /// The content depends on whether the current contact belongs to the "YoungCustomers" persona.
        /// Caches the output for 10 minutes, with different cache versions defined by the "OnlineMarketing" custom string.
        /// The "OnlineMarketing" configuration separately caches each combination of persona and AB test variant variables.
        /// </summary>
        [OutputCache(Duration = 600, VaryByCustom = "OnlineMarketing")]
        public ActionResult PersonalizedGreeting()
        {
            CurrentContactViewModel model;

            // If on-line marketing is disabled, CurrentContact is null
            if (CurrentContact != null)
            {
                model = new CurrentContactViewModel(CurrentContact);
            }
            else
            {
                model = null;
            }

            return View(model);
        }
    }
}