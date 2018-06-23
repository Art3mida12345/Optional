using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Optional.Domain.Core;
using Optional.Infrastructure.Data;
using Optional.Models;

namespace Optional.Controllers
{
    /// <summary>
    /// Controller that realize LogIn and LogOut methods.
    /// </summary>
    public class AccountController : Controller
    {
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        /// <summary>
        /// Login user (Get).
        /// </summary>
        /// <param name="returnUrl">Saves the path from which the user came.</param>
        /// <returns>Returns view "Login".</returns>
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Login in the System (Post)
        /// </summary>
        /// <param name="model">Login model</param>
        /// <param name="returnUrl">Saves the path from which the user came.</param>
        /// <returns>Redirect to returnUrl or View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.Login, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", @"Wrong login or password.");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    if (String.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Logout.
        /// </summary>
        /// <returns>RedirectToAction("Login").</returns>
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}