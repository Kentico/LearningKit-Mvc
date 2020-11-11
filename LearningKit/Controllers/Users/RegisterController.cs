using System;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Kentico.Membership;

using CMS.Core;

namespace LearningKit.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IEventLogService eventLogService;

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

        public RegisterController(IEventLogService eventLogService)
        {
            this.eventLogService = eventLogService;
        }


        /// <summary>
        /// Basic action that displays the registration form.
        /// </summary>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handles creation of new users when the registration form is submitted.
        /// Accepts parameters posted from the registration form via the RegisterViewModel.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            // Validates the received user data based on the view model
            if (!ModelState.IsValid)
            {
                return View(model);
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

            // Attempts to create the user in the Xperience database
            IdentityResult registerResult = IdentityResult.Failed();
            try
            {
                registerResult = await KenticoUserManager.CreateAsync(user, model.Password);
            }
            catch (Exception ex)
            {
                // Logs an error into the Xperience event log if the creation of the user fails
                eventLogService.LogException("MvcApplication", "UserRegistration", ex);
                ModelState.AddModelError(String.Empty, "Registration failed");
            }

            // If the registration was successful, signs in the user and redirects to a different action
            if (registerResult.Succeeded)
            {
                var signInResult = await KenticoSignInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
                
                if (signInResult == SignInStatus.LockedOut)
                {
                    // Checks if the 'Registration requires administrator's approval' settings is enabled
                    if (user.WaitingForApproval)
                    {
                        // Notifies the user that their account is pending administrator approval
                        var message = new IdentityMessage()
                        {
                            Destination = model.Email,
                            Subject = "Account approval pending",
                            Body = "You account is pending administrator approval"
                        };

                        await KenticoUserManager.EmailService.SendAsync(message);
                    }

                    return RedirectToAction("RequireConfirmedAccount");
                }

                return RedirectToAction("Index", "Home");
            }

            // If the registration was not successful, displays the registration form with an error message
            foreach (string error in registerResult.Errors)
            {
                ModelState.AddModelError(String.Empty, error);
            }
            return View(model);            
        }
    }
}