using System.Web.Mvc;
using Optional.Domain.Interfaces;


namespace Optional.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public HomeController(IStudentRepository student)
        {
            _studentRepository = student;
        }

        public ViewResult Index()
        {
            ViewBag.Student = _studentRepository.GetAll();
            return View();
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
            _studentRepository.Dispose();
            base.Dispose(d);
        }
    }
}