using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using Optional.Areas.Lecturer.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Areas.Lecturer.Controllers
{
    /// <summary>
    /// Lecturer`s actions.
    /// </summary>
    [Authorize(Roles = "teacher")]
    public class LecturerController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly ICourseRepository _courseRepository;
        private readonly IRegisterRepository _registerRepository;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor of LecturerController.
        /// </summary>
        public LecturerController(ICourseRepository courseRepository, IRegisterRepository registerRepository)
        {
            _courseRepository = courseRepository;
            _registerRepository = registerRepository;
        }

        /// <summary>
        /// View info about lecturer.
        /// </summary>
        public ActionResult Index()
        {
            Domain.Core.Lecturer user = (Domain.Core.Lecturer)UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                return View(user);
            }
            _logger.Warn("Index method of controller Lecturer: user=null, user received HttpUnauthorizedResult");

            return HttpNotFound();
        }

        /// <returns>All courses from db.</returns>
        public ActionResult CourseList()
        {
            Domain.Core.Lecturer user = (Domain.Core.Lecturer)UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                var courses = _courseRepository.GetAll().ToList();
                courses = courses.Where(course => course.Lecturer.IfNotNull(l => l.UserName.Equals(user.UserName)))
                    .ToList();
                if (courses.Count != 0)
                {
                    return PartialView(courses);
                }

                return new ContentResult { Content = "<p>You are not assigned to any course.</p>" };
            }

            _logger.Warn("CourseList method of controller Lecturer: user=null, user received HttpUnauthorizedResult");
            return HttpNotFound();
        }

        /// <summary>
        /// Returns course`s students with marks.
        /// </summary>
        /// <param name="id">Course id.</param>
        public ActionResult Grade(int id)
        {
            try
            {
                var students = _courseRepository.GetWithStudents(id).Students.ToList();

                if (students.Count == 0)
                {
                    return new ContentResult { Content = "<p>No student is registered on the course.</p>" };
                }

                var studentsWithMarks = new List<StudentWithMark>();
                var registers = _courseRepository.GetMarks(id).ToList();
                foreach (var student in students)
                {
                    studentsWithMarks.Add(new StudentWithMark
                    {
                        BirthDate = student.BirthDate,
                        Email = student.Email,
                        FirstName = student.FirstName,
                        Gender = student.Gender,
                        Group = student.Group,
                        LastName = student.LastName,
                        Mark = registers.FirstOrDefault(r => r.Student.UserName == student.UserName)?.Mark,
                        UserName = student.UserName,
                        MiddleName = student.MiddleName,
                        PhoneNumber = student.PhoneNumber,
                        YearOfStudy = student.YearOfStudy
                    });
                }

                ViewBag.CourseId = id;
                return View(studentsWithMarks);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "User recive HttpNotFound");
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Call create or edit to register.
        /// </summary>
        /// <param name="name">Student name.</param>
        /// <param name="courseId">Id of course.</param>
        [HttpGet]
        public ViewResult GradeMark(string name, int courseId)
        {
            var register = _courseRepository.GetMarks(courseId).FirstOrDefault(r => r.Student.UserName.Equals(name));
            if (register != null)
            {
                return View("EditRegister", new RegisterViewModel
                {
                    RegisterId = register.RegisterId,
                    Mark = register.Mark
                });
            }

            return View("CreateRegister", new RegisterViewModel
            {
                CourseId = courseId,
                StudentName = name
            });
        }

        /// <summary>
        /// Add new register to db.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRegister(RegisterViewModel registerViewModel)
        {
            Register register = new Register
            {
                Mark = registerViewModel.Mark
            };
            _registerRepository.Create(register, registerViewModel.CourseId, registerViewModel.StudentName);
            return RedirectToAction("Grade", new { id = registerViewModel.CourseId });
        }

        /// <summary>
        /// Edit register from db.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRegister(RegisterViewModel registerViewModel)
        {
            Register register = _registerRepository.Get(registerViewModel.RegisterId);
            register.Mark = registerViewModel.Mark;
            _registerRepository.Update(register);
            return RedirectToAction("Grade", new { id = register.Course.CourseId });
        }

        /// <returns>Returns all students of course.</returns>
        [HttpGet]
        public ActionResult ViewStudentsOfCourse(int courseId)
        {
            try
            {
                var students = _courseRepository.GetWithStudents(courseId).Students.ToList();
                if (students.Count != 0)
                {
                    return View(students);
                }
                else
                {
                    return new ContentResult { Content = "<p>No student is registered on the course.</p>" };
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "User recive httpnotfound.");
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Calls dispose for repositories.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            _courseRepository.Dispose();
            _registerRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}