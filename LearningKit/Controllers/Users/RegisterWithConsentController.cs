using System;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Kentico.Membership;

using CMS.ContactManagement;
using CMS.Core;
using CMS.DataProtection;
using CMS.Membership;

namespace LearningKit.Controllers
{
    public class RegisterWithConsentController : Controller
    {
        private readonly IEventLogService eventLogService;
        private readonly IFormConsentAgreementService formConsentAgreementService;
        private readonly IUserInfoProvider userInfoProvider;
        private readonly ConsentInfo consent;

        /// <summary>
        /// Constructor.
        /// You can use a dependency injection container to initialize required services and providers.
        /// </summary>
        public RegisterWithConsentController(IEventLogService eventLogService,
                                             IFormConsentAgreementService formConsentAgreementService,
                                             IUserInfoProvider userInfoProvider,
                                             IConsentInfoProvider consentInfoProvider)
        {
            this.eventLogService = eventLogService;
            this.formConsentAgreementService = formConsentAgreementService;
            this.userInfoProvider = userInfoProvider;

            // Gets the related consent
            // Fill in the code name of the appropriate consent object in Kentico
            consent = consentInfoProvider.Get("SampleRegistrationConsent");
        }


        /// <summary>
        /// Provides access to the Kentico.Membership.KenticoSignInManager instance.
        /// </summary>
        public KenticoSignInManager KenticoSignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<KenticoSignInManager>();
            }
        }


        /// <summary>
        /// Provides access to the Kentico.Membership.KenticoUserManager instance.
        /// </summary>
        public KenticoUserManager KenticoUserManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<KenticoUserManager>();
            }
        }


        /// <summary>
        /// Basic action that displays the registration form.
        /// </summary>
        public ActionResult Register()
        {
            var model = new RegisterWithConsentViewModel
            {
                // Adds the consent text to the registration model
                ConsentShortText = consent.GetConsentText("en-US").ShortText,
                ConsentIsAgreed = false
            };

            return View("RegisterWithConsent", model);
        }


        /// <summary>
        /// Handles creation of new users and consent agreements when the registration form is submitted.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Register(RegisterWithConsentViewModel model)
        {
            // Validates the received user data based on the view model
            if (!ModelState.IsValid)
            {
                model.ConsentShortText = consent.GetConsentText("en-US").ShortText;
                return View("RegisterWithConsent", model);
            }

            // Prepares a new user entity using the posted registration data
            Kentico.Membership.User user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Enabled = true // Enables the new user directly
            };

            // Attempts to create the user in the Kentico database
            IdentityResult registerResult = IdentityResult.Failed();
            try
            {
                registerResult = await KenticoUserManager.CreateAsync(user, model.Password);
            }
            catch (Exception ex)
            {
                // Logs an error into the Kentico event log if the creation of the user fails
                eventLogService.LogException("MvcApplication", "UserRegistration", ex);
                ModelState.AddModelError(String.Empty, "Registration failed");
            }

            // If the registration was not successful, displays the registration form with an error message
            if (!registerResult.Succeeded)
            {
                foreach (string error in registerResult.Errors)
                {
                    ModelState.AddModelError(String.Empty, error);
                }

                model.ConsentShortText = consent.GetConsentText("en-US").ShortText;

                return View("RegisterWithConsent", model);
            }

            // Creates a consent agreement if the consent checkbox was selected in the registration form
            if (model.ConsentIsAgreed)
            {
                // Gets the current contact
                var currentContact = ContactManagementContext.GetCurrentContact();

                // Creates an agreement for the specified consent and contact
                // Passes the UserInfo object of the new user as a parameter, which is used to map the user's values
                // to a new contact in cases where the contact parameter is null,
                // e.g. for visitors who have not given an agreement with the site's tracking consent.
                formConsentAgreementService.Agree(currentContact, consent, userInfoProvider.Get(user.Id));
            }

            // If the registration was successful, signs in the user and redirects to a different action
            await KenticoSignInManager.SignInAsync(user, true, false);
            return RedirectToAction("Index", "Home");
        }
    }
}