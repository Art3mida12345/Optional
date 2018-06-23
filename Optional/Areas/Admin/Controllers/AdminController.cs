using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using Optional.Areas.Admin.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Areas.Admin.Controllers
{
    /// <summary>
    /// Methods for Admin.
    /// </summary>
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICourseRepository _courseRepository;

        /// <summary>
        /// Constructor of AdminController
        /// </summary>
        /// <param name="course">Repository of course.</param>
        public AdminController(ICourseRepository course)
        {
            _courseRepository = course;
            _logger.Info("Constructor of AdminComtroller");
        }
        /// <summary>
        /// Home page for Admin show brief info for user about him.
        /// </summary>
        public ActionResult Index()
        {
            ApplicationUser user = UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                return View(user);
            }

            _logger.Warn("Index method of controller Admin: user wasn`t found.");
            return View();
        }

        /// <returns>Returns view RegisterLecturer.</returns>
        public ActionResult RegisterLecturer()
        {
            return View();
        }
        /// <summary>
        /// Post method register lecturer. Add him to db.
        /// </summary>
        /// <param name="model">Data from registered form.</param>
        [HttpPost]
        public async Task<ActionResult> RegisterLecturer(LecturerRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new Domain.Core.Lecturer()
                {
                    BirthDate = model.BirthDate,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    PhoneNumber = model.Phone,
                    UserName = model.Login,
                    Department = model.Department,
                    Email = model.Email
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
                    _logger.Error("RegisterLecturer method of controller Admin: Error creating a user.", error);
                }
            }
            return View(model);
        }
        /// <summary>
        /// View form for creating course.
        /// </summary>
        [HttpGet]
        public ViewResult CreateCourse()
        {
            //read all lecturers from db
            var lecturers = UserManager.Users.OfType<Domain.Core.Lecturer>().ToList();
            ViewBag.Lecturers = new SelectList(lecturers, "UserName", "UserName");
            return View();
        }

        /// <summary>
        /// Add course to db.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse(CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                var duration = course.EndDate - course.StartDate;
                _courseRepository.Create(new Course
                {
                    Duration = duration.Days,
                    StartDate = course.StartDate,
                    Theme = course.Theme,
                    Title = course.Title
                });
                if (course.LecturerName != null)
                {
                    _courseRepository.AddLecturerToCourse(course.LecturerName,
                        _courseRepository.GetAll().Last().CourseId);
                }

                return RedirectToAction("Index");
            }

            var users = UserManager.Users.OfType<Domain.Core.Lecturer>();
            ViewBag.Lecturers = new SelectList(users, "UserName", "UserName");
            return View(course);
        }

        /// <summary>
        /// Remove student from role active.
        /// </summary>
        /// <param name="name">Student`s name.</param>
        /// <returns>RedirectToAction("BlockUnblockStudent").</returns>
        public ActionResult BlockStudent(string name)
        {
            UserManager.RemoveFromRole(UserManager.FindByName(name).Id, "active");
            return RedirectToAction("BlockUnblockStudent");
        }

        /// <summary>
        /// Add student to role active.
        /// </summary>
        /// <param name="name">Student`s name.</param>
        /// <returns>RedirectToAction("BlockUnblockStudent").</returns>
        public ActionResult UnblockStudent(string name)
        {
            UserManager.AddToRole(UserManager.FindByName(name).Id, "active");
            return RedirectToAction("BlockUnblockStudent");
        }

        /// <returns>Redirect to Index if students count =0.</returns>
        public ViewResult BlockUnblockStudent()
        {
            var students = UserManager.Users?.OfType<Domain.Core.Student>().ToList();

            if (students.Count > 0)
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

                return View(blockStudent);
            }

            return View("Index");
        }

        /// <summary>
        /// View all lecturers from db.
        /// </summary>
        public ViewResult LecturerList()
        {
            var lecturers = UserManager.Users.OfType<Domain.Core.Lecturer>().ToList();

            return View(lecturers);
        }

        /// <summary>
        /// Gets all course, that don`t have a lecturer to AddLecturerToCourse method.
        /// </summary>
        public ViewResult SelectLecturerToCourse(string lecturerName)
        {
            ViewBag.Lecturer = lecturerName;
            return View("AddLecturerToCourse",_courseRepository.GetAll().Where(c => c.Lecturer == null).ToList());
        }

        /// <param name="courseId"></param>
        /// <param name="lecturerName"></param>
        /// <returns>Returns RedirectToAction("Index").</returns>
        public ActionResult AddLecturerToCourse(int courseId, string lecturerName)
        {
            _courseRepository.AddLecturerToCourse(lecturerName,courseId);
            return RedirectToAction("Index");
        }

        /// <param name="id">Id of edited course.</param>
        [HttpGet]
        public ActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            try
            {
                Course course = _courseRepository.Get((int) id);
                if (course != null)
                {
                    CourseViewModel courseView = new CourseViewModel
                    {
                        CourseId = course.CourseId,
                        EndDate = course.StartDate.AddDays(course.Duration),
                        StartDate = course.StartDate,
                        Theme = course.Theme,
                        Title = course.Title
                    };

                    return View(courseView);
                }
            }
            catch(NullReferenceException ex)
            {
                _logger.Error("User recive HttpNotFound", ex);
                return HttpNotFound();
            }

            return HttpNotFound();
        }

        /// <summary>
        /// Edit course in db.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                var duration = course.EndDate - course.StartDate;
                var editCourse = _courseRepository.Get(course.CourseId);
                editCourse.Duration = duration.Days;
                editCourse.StartDate = course.StartDate;
                editCourse.Title = course.Title;
                editCourse.Theme = course.Theme;

                _courseRepository.Update(editCourse);
                return RedirectToAction("CourseList");
            }

            return View(course);
        }

        /// <returns>All courses from db.</returns>
        public ActionResult CourseList()
        {
            return View(_courseRepository.GetAll().ToList());
        }

        /// <summary>
        /// Delete course action.
        /// </summary>
        /// <returns>View DeleteCourseConfirmed.</returns>
        [HttpGet]
        public ActionResult DeleteCourse(int id)
        {
            try
            {
                Course course = _courseRepository.Get(id);
                if (course == null)
                {
                    _logger.Warn("DeleteCourse method of controller Admin: Course wasn`t found.");
                    return HttpNotFound();
                }
                return View(course);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,"User recive model again");
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Delete coures from db.
        /// </summary>
        [HttpPost, ActionName("DeleteCourse")]
        public ActionResult DeleteCourseConfirmed(int id)
        {
            try
            {
                Course course = _courseRepository.Get(id);
                if (course == null)
                {
                    _logger.Warn("DeleteCourseConfirmd method of controller Admin: Course wasn`t found.");
                    return HttpNotFound();
                }

                _courseRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex,"User Redirect and method not work");
            }

            return RedirectToAction("CourseList");
        }

        /// <returns>All lecturers from db.</returns>
        public ViewResult SelectLecturer(int courseId)
        {
            var users = UserManager.Users.ToList();
            var lecturers = UserManager.Users.OfType<Domain.Core.Lecturer>().ToList();

            ViewBag.CourseId = courseId;
            return View(lecturers);
        }

        /// <summary>
        /// Dispose course repository and call base.
        /// </summary>
        protected override void Dispose(bool d)
        {
            _courseRepository.Dispose();
            base.Dispose(d);
        }
    }
}