using System;
using System.Linq;
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
    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;

        public StudentController(ICourseRepository courseRepository, IStudentRepository studentRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
        }

        public ActionResult Index()
        {
            Domain.Core.Student user = (Domain.Core.Student) UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                if (User.IsInRole("active"))
                {
                    ViewBag.Active = "Профиль активен.";
                    return View(user);
                }

                ViewBag.Active = "Профиль заблокирован администратором.";
                return View(user);
            }

            return HttpNotFound();
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

        public ActionResult NotStartedCourses()
        {
            Domain.Core.Student user = _studentRepository.Get(User.Identity.Name);
            if (user != null)
            {
                var courses = user.Courses.Where(course => course.StartDate.CompareTo(DateTime.Now)==1).ToList();
                if (courses.Count == 0)
                {
                    return new ContentResult { Content = "<p>Таких курсов нет.</p>" };
                }

                return PartialView("CoursesList",courses);
            }

            return new ContentResult{Content = "<p>Таких курсов нет.</p>"};
        }

        public ActionResult StartedCourses()
        {
            Domain.Core.Student user = _studentRepository.Get(User.Identity.Name);
            if (user != null)
            {
                var courses = user.Courses.Where(course => course.StartDate.CompareTo(DateTime.Now) == -1).ToList();
                if (courses.Count == 0)
                {
                    return new ContentResult { Content = "<p>Таких курсов нет.</p>" };
                }

                return PartialView("CoursesList", courses);
            }

            return new ContentResult { Content = "<p>Таких курсов нет.</p>" };
        }

        public ActionResult PassedCourses()
        {
            Domain.Core.Student user = _studentRepository.GetWithRegisters(User.Identity.Name);
            if (user != null)
            {
                var registers = user.Registers.ToList();
            }
            return new ContentResult { Content = "<p>Таких курсов нет.</p>" };
        }

        protected override void Dispose(bool d)
        {
            _courseRepository.Dispose();
            _studentRepository.Dispose();
            base.Dispose(d);
        }
    }
}