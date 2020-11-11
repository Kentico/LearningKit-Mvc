using CMS.ContactManagement;
using CMS.Core;
using CMS.DataProtection;



using LearningKit.Personalization.ConditionTypes;

//DocSection:ConditionTypeRegistration
using Kentico.PageBuilder.Web.Mvc.Personalization;

[assembly: RegisterPersonalizationConditionType("LearningKit.Personalization.HasGivenConsentConditionTypeCustom", 
    typeof(HasGivenConsentConditionTypeCustom), "Has given consent agreement (custom)", 
    ControllerType = typeof(HasGivenConsentController), 
    Description = "Evaluates whether the contact has given an agreement with a specified consent declaration.", 
    IconClass = "icon-clipboard-checklist",
    Hint = "Enter the code name of a consent. The condition is fulfilled for visitors who have given an agreement with the given consent.")]
//EndDocSection:ConditionTypeRegistration

namespace LearningKit.Personalization.ConditionTypes
{
    public class HasGivenConsentConditionTypeCustom : ConditionType
    {
        // Parameter: Code name that identifies the consent for which visitors need to give an agreement to fulfill the condition
        public string ConsentCodeName { get; set; }

        /// <summary>
        /// Default property representing the name of the personalization variant.
        /// </summary>
        public override string VariantName
        {
            get
            {
                // Uses the specified consent code name as the name of the variant
                return ConsentCodeName;
            }
            set
            {
                // No need to set the variant name property
            }
        }

        public override bool Evaluate()
        {
            // Gets the contact object of the current visitor
            ContactInfo currentContact = ContactManagementContext.GetCurrentContact(false);

            // Creates an instance of the consent agreement service
            // For real-world projects, we recommend using a dependency injection container to initialize service instances
            var consentAgreementService = Service.Resolve<IConsentAgreementService>();

            // Gets the consent object based on its code name
            ConsentInfo consent = ConsentInfo.Provider.Get(ConsentCodeName);
            if (consent == null || currentContact == null)
            {
                return false;
            }

            // Checks if the contact has given a consent agreement
            return consentAgreementService.IsAgreed(currentContact, consent);
        }
    }
}