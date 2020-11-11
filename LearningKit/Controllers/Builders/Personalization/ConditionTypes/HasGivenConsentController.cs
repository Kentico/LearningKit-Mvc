using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc.Personalization;

namespace LearningKit.Personalization.ConditionTypes
{
    //DocSection:ConditionTypeController
    public class HasGivenConsentController : ConditionTypeController<HasGivenConsentConditionType>
    {
        // Displays the configuration dialog
        [HttpPost]
        public ActionResult Index()
        {
            // Gets the condition's current configuration as an instance of the condition type class
            var conditionType = GetParameters();
            
            // Creates a view model object
            var viewModel = new HasGivenConsentViewModel
            {
                // Sets the consent code name obtained from the condition type parameters
                ConsentCodeName = conditionType.ConsentCodeName
            };

            // Displays the configuration dialog's view
            return PartialView("Personalization/ConditionTypes/_HasGivenConsentConfiguration", viewModel);
        }

        // Submits the condition type parameters
        [HttpPost]
        public ActionResult Validate(HasGivenConsentViewModel model)
        {
            // Validates the model
            if (!ModelState.IsValid)
            {
                return PartialView("Personalization/ConditionTypes/_HasGivenConsentConfiguration", model);
            }

            // Creates an object of the condition type class
            var parameters = new HasGivenConsentConditionType
            {
                ConsentCodeName = model.ConsentCodeName,
            };

            // Serializes the condition's configuration into JSON format and returns the data
            return new ConditionTypeValidationResult(parameters);
        }
    }
    //EndDocSection:ConditionTypeController
}