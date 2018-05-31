using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Optional.Areas.Admin.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly ICourseRepository _courseRepository;

        public AdminController(ICourseRepository course)
        {
            _courseRepository = course;
        }

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

        [HttpGet]
        public ViewResult CreateCourse()
        {
            var users = UserManager.Users.ToList();
            var lecturers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if(UserManager.IsInRole(user.Id,"teacher"))
                    lecturers.Add(user);
            }
            ViewBag.Lecturers = new SelectList(lecturers, "UserName", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult CreateCourse(CourseViewModel course)
        {
            if(ModelState.IsValid){
                _courseRepository.Create(new Course
            {
                Duration = course.Duration,
                StartDate = course.StartDate,
                Theme = course.Theme,
                Title = course.Title
            });
                return RedirectToAction("Index");
            }
            return View(course);
        }

        public ActionResult BlockStudent(string name)
        {
            UserManager.RemoveFromRole(UserManager.FindByName(name).Id, "active");
            return RedirectToAction("BlockUnblockStudent");
        }

        public ActionResult UnblockStudent(string name)
        {
            UserManager.AddToRole(UserManager.FindByName(name).Id, "active");
            return RedirectToAction("BlockUnblockStudent");
        }

        public ViewResult BlockUnblockStudent()
        {
            List<Student> students =
                UserManager.Users.Where(u => UserManager.IsInRole(u.Id, "student")).Cast<Student>().ToList();

            if (students.Count>0)
            {
                List<BlockStudentViewModel> blockStudent=new List<BlockStudentViewModel>();
                foreach (var student in students)
                {
                    blockStudent.Add(new BlockStudentViewModel
                    {
                        Blocked = UserManager.IsInRole(student.Id, "active"),
                        FirstName = student.FirstName,
                        Group = student.Group,
                        UserName = student.UserName,
                        MiddleName = student.MiddleName,
                        LastName = student.LastName,
                        YearOfStudy = student.YearOfStudy
                    });
                }

                ViewBag.Students = blockStudent;
                return View();
            }

            return View("Index");
        }

        public ViewResult LecturerList()
        {
            var users = UserManager.Users.ToList();
            var lecturers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (UserManager.IsInRole(user.Id, "teacher"))
                    lecturers.Add(user);
            }

            return View(lecturers.Cast<Lecturer>().ToList());
        }

        public ViewResult SelectLecturerToCourse(string lecturerName)
        {
            ViewBag.Lecturer = lecturerName;
            return View("AddLecturerToCourse",_courseRepository.GetAll().Where(c => c.Lecturer == null).ToList());
        }

        public ActionResult AddLecturerToCourse(int courseId, string lecturerName)
        {
            _courseRepository.AddLecturerToCourse(lecturerName,courseId);
            return RedirectToAction("Index");
        }
    }
}