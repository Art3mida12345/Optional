using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public HomeController(IStudentRepository student)
        {
            _studentRepository = student;
        }

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ApplicationUser user = new ApplicationUser() {Email = "email@.com", UserName = "Admin"};
            string password = "verylong&hardpassword";
            UserManager.Create(user, password);
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool d)
        {
            //_studentRepository.Dispose();
            base.Dispose(d);
        }
    }
}