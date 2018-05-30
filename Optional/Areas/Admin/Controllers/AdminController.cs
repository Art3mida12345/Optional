using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Optional.Areas.Admin.Models;
using Optional.Domain.Core;
using Optional.Infrastructure.Data;

namespace Optional.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private ApplicationRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterLecturer()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterLecturer(LecturerRegisterModel model)
        {
            RoleManager.Create(new ApplicationRole { Name = "lecturer" });
            if (ModelState.IsValid)
            {
                ApplicationUser user = new Lecturer()
                {
                    BirthDate = model.BirthDate,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    PhoneNumber = model.Phone,
                    UserName = model.Login,
                    Department = model.Department
                };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "lecturer");
                    return RedirectToAction("Login", "Account", new {area=""});
                }

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
    }
}