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
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly ICourseRepository _courseRepository;
        //private ApplicationRoleManager RoleManager=> HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
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
                    UserManager.AddToRole(user.Id, "teacher");
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
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse(CourseViewModel course)
        {
            if(ModelState.IsValid)
            {
                _courseRepository.Create(new Course
            {
                Duration = course.Duration,
                StartDate = course.StartDate,
                Theme = course.Theme,
                Title = course.Title,
            });
                if (course.LecturerName != null)
                {
                    _courseRepository.AddLecturerToCourse(course.LecturerName, 
                        _courseRepository.GetAll().Last().CourseId);
                }
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
            var users = UserManager.Users.ToList();
            var students = new List<Domain.Core.Student>();
            foreach (var user in users)
            {
                if (UserManager.IsInRole(user.Id, "student"))
                    students.Add((Domain.Core.Student)user);
            }

            if (users.Count > 0)
            {
                List<BlockStudentViewModel> blockStudent = new List<BlockStudentViewModel>();
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

        [HttpGet]
        public ActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Course course = _courseRepository.Get((int) id);
            if (course != null)
            {
                CourseViewModel courseView = new CourseViewModel
                {
                    CourseId = course.CourseId,
                    Duration = course.Duration,
                    StartDate = course.StartDate,
                    Theme = course.Theme,
                    Title = course.Title
                };

                return View(courseView);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(CourseViewModel course)
        {
            _courseRepository.Update(_courseRepository.Get(course.CourseId));
            return RedirectToAction("CourseList");
        }

        public ActionResult CourseList()
        {
            return View(_courseRepository.GetAll().ToList());
        }

        [HttpGet]
        public ActionResult DeleteCourse(int id)
        {
            Course course = _courseRepository.Get(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [HttpPost, ActionName("DeleteCourse")]
        public ActionResult DeleteCourseConfirmed(int id)
        {
            Course course = _courseRepository.Get(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            _courseRepository.Delete(id);
            return RedirectToAction("CourseList");
        }

        public ViewResult SelectLecturer(int courseId)
        {
            var users = UserManager.Users.ToList();
            var lecturers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (UserManager.IsInRole(user.Id, "teacher"))
                    lecturers.Add(user);
            }

            ViewBag.CourseId = courseId;
            return View(lecturers.Cast<Lecturer>().ToList());
        }

        protected override void Dispose(bool d)
        {
            _courseRepository.Dispose();
            base.Dispose(d);
        }
    }
}