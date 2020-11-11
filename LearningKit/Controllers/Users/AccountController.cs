//DocSection:Using
using System;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Kentico.Membership;

using CMS.Core;
using CMS.Base.UploadExtensions;
using CMS.Membership;
using CMS.SiteProvider;
//EndDocSection:Using

namespace LearningKit.Controllers
{
    public class AccountController : Controller
    {
        //DocSection:DependencyInjection
        private readonly IAvatarService avatarService;
        private readonly IEventLogService eventLogService;

        public AccountController(IAvatarService avatarService,
                                 IEventLogService eventLogService)
        {
            // Initializes an instance of a service used to manage user avatars
            this.avatarService = avatarService;
            this.eventLogService = eventLogService;
        }
        //EndDocSection:DependencyInjection


        //DocSection:SignIn
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
        /// Basic action that displays the sign-in form.
        /// </summary>
        public ActionResult SignIn()
        {
            return View();
        }     
        
        
        /// <summary>
        /// Handles authentication when the sign-in form is submitted. Accepts parameters posted from the sign-in form via the SignInViewModel.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> SignIn(SignInViewModel model, string returnUrl)
        {
            // Validates the received user credentials based on the view model
            if (!ModelState.IsValid)
            {
                // Displays the sign-in form if the user credentials are invalid
                return View();
            }
            
            // Attempts to authenticate the user against the Xperience database
            SignInStatus signInResult = SignInStatus.Failure;
            try
            {
                signInResult = await KenticoSignInManager.PasswordSignInAsync(model.UserName, model.Password, model.SignInIsPersistent, false);
            }
            catch (Exception ex)
            {
                // Logs an error into the Xperience event log if the authentication fails
                eventLogService.LogException("MvcApplication", "SignIn", ex);
            }            

            // If the authentication was successful, redirects to the return URL when possible or to a different default action
            if (signInResult == SignInStatus.Success)
            {                
                string decodedReturnUrl = Server.UrlDecode(returnUrl);
                if (!string.IsNullOrEmpty(decodedReturnUrl) && Url.IsLocalUrl(decodedReturnUrl))
                {
                    return Redirect(decodedReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            // If the 'Registration requires administrator's approval' setting is enabled and the user account
            // is pending activation, displays an appropriate message
            if (signInResult == SignInStatus.LockedOut)
            {
                // If the 'Registration requires administrator's approval' setting is enabled and the user account
                // is pending activation, displays an appropriate message
                User user = await KenticoUserManager.FindByNameAsync(model.UserName);
                if (user.WaitingForApproval)
                {
                    ModelState.AddModelError(String.Empty, "You account is pending administrator approval.");
                }

                return View();
            }

            // If the authentication was not successful, displays the sign-in form with an "Authentication failed" message 
            ModelState.AddModelError(String.Empty, "Authentication failed");
            return View();
        }
        //EndDocSection:SignIn


        //DocSection:Signout
        /// <summary>
        /// Provides access to the Microsoft.Owin.Security.IAuthenticationManager instance.
        /// </summary>
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        

        /// <summary>
        /// Action for signing out users. The Authorize attribute allows the action only for users who are already signed in.
        /// </summary>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            // Signs out the current user
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            
            // Redirects to a different action after the sign-out
            return RedirectToAction("Index", "Home");
        }
        //EndDocSection:Signout


        //DocSection:EditUser
        /// <summary>
        /// Provides access to user related API which will automatically save changes to the database using Kentico.Membership.KenticoUserStore.
        /// </summary>
        public KenticoUserManager KenticoUserManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<KenticoUserManager>();
            }
        }

        
        /// <summary>
        /// Displays a form where user information can be changed.
        /// </summary>
        public ActionResult EditUser(bool avatarUpdateFailed = false)
        {
            // Finds the user based on their current user name
            User user = KenticoUserManager.FindByName(User.Identity.Name);

            EditUserAccountViewModel model = new EditUserAccountViewModel() 
            {
                User = user,
                AvatarUpdateFailed = avatarUpdateFailed
            };
            
            return View(model);
        }

        
        /// <summary>
        /// Saves the entered changes of the user details to the database.
        /// </summary>
        /// <param name="returnedUser">User that is changed.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> EditUser(User returnedUser)
        {
            // Finds the user based on their current user name
            User user = KenticoUserManager.FindByName(User.Identity.Name);
            
            // Assigns the names based on the entered data
            user.FirstName = returnedUser.FirstName;
            user.LastName = returnedUser.LastName;

            // Saves the user details into the database
            await KenticoUserManager.UpdateAsync(user);

            return RedirectToAction("Index", "Home");
        }
        //EndDocSection:EditUser


        //DocSection:UserAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangeAvatar(HttpPostedFileBase avatarUpload)
        {
            object routevalues = null;

            // Checks that the avatar file was uploaded
            if (avatarUpload != null && avatarUpload.ContentLength > 0)
            {   
                // Gets the representation of the user requesting avatar change from Xperience
                var user = KenticoUserManager.FindByName(User.Identity.Name);
                // Attempts to update the user's avatar
                if (!avatarService.UpdateAvatar(avatarUpload.ToUploadedFile(), user.Id, SiteContext.CurrentSiteName))
                {
                    // If the avatar update failed (e.g., the user uploaded an image with an usupported file extension)
                    // sets a flag for an error message in the corresponding view
                    routevalues = new { avatarUpdateFailed = true };
                }
            }

            return RedirectToAction("EditUser", "Account", routevalues);
        }
        //EndDocSection:UserAvatar


        //DocSection:DeleteAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult DeleteAvatar(int userId)
        {
            // Deletes the avatar image associated with the user
            avatarService.DeleteAvatar(userId);

            return RedirectToAction("EditUser", "Account");
        }
        //EndDocSection:DeleteAvatar
    }
}