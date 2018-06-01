using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Optional.Areas.Student.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Areas.Student.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly ICourseRepository _courseRepository;

        public StudentController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        private ApplicationRoleManager RoleManager=> HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "student")]
        public ActionResult EnrollForCourse(int id)
        {
            Domain.Core.Student user = (Domain.Core.Student) UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (User.IsInRole("active"))
            {
                if (user != null)
                {
                    var course = _courseRepository.Get(id);
                    if (course != null)
                    {
                            _courseRepository.AddStudentToCourse(user.UserName, id);
                            UserManager.Update(user);
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", new {controller = "Account", area = ""});
            }

            return RedirectToAction("Index", new { controller = "Home", area="" });
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(StudentViewModel studentView)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser student = new Domain.Core.Student
                {
                    BirthDate = studentView.BirthDate,
                    FirstName = studentView.FirstName,
                    Gender = studentView.Gender,
                    Group = studentView.Group,
                    LastName = studentView.LastName,
                    MiddleName = studentView.MiddleName,
                    PhoneNumber = studentView.PhoneNumber,
                    UserName = studentView.UserName,
                    YearOfStudy = studentView.YearOfStudy
                };
                var result = await UserManager.CreateAsync(student, studentView.Password);

                if (result.Succeeded)
                {
                    //RoleManager.Create(new ApplicationRole {Name = "student"});
                    //RoleManager.Create(new ApplicationRole {Name = "active"});

                    UserManager.AddToRoles(student.Id, "student", "active");

                    return RedirectToAction("Login", "Account", new {area = ""});
                }

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(studentView);
        }

        protected override void Dispose(bool d)
        {
            _courseRepository.Dispose();
            base.Dispose(d);
        }

    }
}