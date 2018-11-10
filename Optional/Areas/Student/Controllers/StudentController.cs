using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using Optional.Areas.Student.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Areas.Student.Controllers
{
    /// <summary>
    /// Actions to students.
    /// </summary>
    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private ApplicationUserManager UserManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRegisterRepository _registerRepository;

        /// <summary>
        /// Constructor of StudentController.
        /// </summary>
        public StudentController(ICourseRepository courseRepository, IStudentRepository studentRepository,
            IRegisterRepository registerRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _registerRepository = registerRepository;
        }

        /// <summary>
        /// View info about student and his courses. View contains partialView.
        /// </summary>
        public ActionResult Index()
        {
            Domain.Core.Student user = (Domain.Core.Student) UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                if (User.IsInRole("active"))
                {
                    ViewBag.Active = "Profile is active";
                    return View(user);
                }

                ViewBag.Active = "Profile is blocked by Admin";
                return View(user);
            }
            _logger.Warn("Index method of controller Student: user=null, user received HttpNotFound");
            return HttpNotFound();
        }

        /// <summary>
        /// Add student to course.
        /// </summary>
        /// <param name="id">Course id.</param>
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
                else
                {
                    _logger.Warn("EnrollForCourse method of controller Student: user=null, user received HttpUnauthorizedResult");
                    return new HttpUnauthorizedResult();
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Register in the system view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register new student in the system.
        /// </summary>
        /// <param name="studentView"></param>
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
                    YearOfStudy = studentView.YearOfStudy,
                    Email = studentView.Email
                };
                var result = await UserManager.CreateAsync(student, studentView.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRoles(student.Id, "student", "active");
                    _logger.Info($"New student {student.UserName} register in the system.");

                    return RedirectToAction("Login", "Account", new {area = ""});
                }
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                    _logger.Error("Register method of controller Student: Error creating a user.", error);
                }
            }

            return View(studentView);
        }

        /// <returns>Returns not started courses of student.</returns>
        public ActionResult NotStartedCourses()
        {
            Domain.Core.Student user = _studentRepository.GetWithCourses(User.Identity.Name);
            if (user != null)
            {
                var courses = user.Courses.Where(course => course.StartDate.CompareTo(DateTime.Now) == 1).ToList();
                if (courses.Count == 0)
                {
                    return new ContentResult {Content = "<p>There are no such courses.</p>"};
                }

                foreach (var course in courses)
                {
                    course.Lecturer = _courseRepository.GetWithLecturer(course.CourseId)?.Lecturer;
                }

                return PartialView("CoursesList", courses);
            }

            return new ContentResult {Content = "<p>There are no such courses.</p>"};
        }

        /// <returns>Returns started courses of student.</returns>
        public ActionResult StartedCourses()
        {
            Domain.Core.Student user = _studentRepository.GetWithCourses(User.Identity.Name);
            if (user != null)
            {
                var courses = user.Courses.Where(course =>
                    course.StartDate.CompareTo(DateTime.Now) <= 0 &&
                    course.StartDate.AddDays(course.Duration).CompareTo(DateTime.Now) >= 1).ToList();
                if (courses.Count == 0)
                {
                    return new ContentResult {Content = "<p>There are no such courses.</p>" };
                }

                foreach (var course in courses)
                {
                    course.Lecturer = _courseRepository.GetWithLecturer(course.CourseId)?.Lecturer;
                }

                return PartialView("CoursesList", courses);
            }

            return new ContentResult {Content = "<p>There are no such courses.</p>"};
        }

        /// <returns>Returns passed courses of student.</returns>
        public ActionResult PassedCourses()
        {
            Domain.Core.Student user = _studentRepository.GetWithCourses(User.Identity.Name);
            if (user != null)
            {
                var courses = new List<PassedCourse>();
                var passedCourses = user.Courses
                    .Where(c => c.StartDate.AddDays(c.Duration).CompareTo(DateTime.Now) <= 0).ToList();
                if (passedCourses.Count != 0)
                {
                    foreach (var course in passedCourses)
                    {
                        var mark = _registerRepository.GetMarkOfStudent(course.CourseId, user.UserName);
                        courses.Add(new PassedCourse
                        {
                            CourseId = course.CourseId,
                            Duration = course.Duration,
                            Mark = (mark == 0) ? null : (int?) mark,
                            StartDate = course.StartDate,
                            Theme = course.Theme,
                            Title = course.Title,
                            Lecturer = _courseRepository.GetWithLecturer(course.CourseId).Lecturer
                        });
                    }

                    return PartialView(courses);
                }
            }

            return new ContentResult {Content = "<p>There are no such courses.</p>"};
        }

        /// <returns>Returns data of edited student.</returns>
        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                Domain.Core.Student student = _studentRepository.Get(User.Identity.Name);
                if (student != null)
                {
                    StudentEditModel model = new StudentEditModel
                    {
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        MiddleName = student.MiddleName,
                        BirthDate = student.BirthDate,
                        PhoneNumber = student.PhoneNumber,
                        Group = student.Group,
                        YearOfStudy = student.YearOfStudy,
                        Email = student.Email
                    };
                    return View(model);
                }

                _logger.Warn("Get_Method of Student Controller user=null. User recive httpnotfound.");
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex,"Student Redirect to LoginPage");
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Edit student in db.
        /// </summary>
        [HttpPost]
        public ActionResult Edit(StudentEditModel model)
        {
            try
            {
                Domain.Core.Student student = _studentRepository.Get(User.Identity.Name);
                if (student != null)
                {
                    student.FirstName = model.FirstName;
                    student.MiddleName = model.MiddleName;
                    student.Group = model.Group;
                    student.YearOfStudy = model.YearOfStudy;
                    student.BirthDate = model.BirthDate;
                    student.Email = model.Email;
                    student.LastName = model.LastName;
                    student.PhoneNumber = model.PhoneNumber;

                    _studentRepository.Update(student);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", @"User isn`t found");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", @"Something went wrong");
                _logger.Error(ex, "Edit (POST) method of controller Student: Error edit a user.", ex);
            }

            return View(model);
        }

        /// <returns>Returns information about the course lecturer.</returns>
        public ActionResult LecturerDetails(int id)
        {
            try
            {
                Course course = _courseRepository.GetWithLecturer(id);
                if (course != null)
                {
                    return View(course.Lecturer);
                }

                _logger.Warn(
                    "LecturerDetails method of controller Student: Course wasn`t found. User received HttpNotFound.");
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "User recive HttpNotFound");
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Call dispose of repositories.
        /// </summary>
        protected override void Dispose(bool d)
        {
            _courseRepository.Dispose();
            _studentRepository.Dispose();
            _registerRepository.Dispose();
            base.Dispose(d);
        }
    }
}