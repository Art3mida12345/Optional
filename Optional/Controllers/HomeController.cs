using System.Linq;
using System.Web.Mvc;
using Optional.Domain.Interfaces;

namespace Optional.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        //private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public HomeController(ICourseRepository course)
        {
            _courseRepository = course;
        }

        public ViewResult Index()
        {
            return View(_courseRepository.GetAll().ToList());
        }

        public ActionResult About()
        {
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
            _courseRepository.Dispose();
            base.Dispose(d);
        }
    }
}