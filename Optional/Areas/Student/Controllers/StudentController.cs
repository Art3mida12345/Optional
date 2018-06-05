﻿using System;
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
    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private ApplicationUserManager UserManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRegisterRepository _registerRepository;

        public StudentController(ICourseRepository courseRepository, IStudentRepository studentRepository,
            IRegisterRepository registerRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _registerRepository = registerRepository;
        }

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
                    YearOfStudy = studentView.YearOfStudy,
                    Email = studentView.Email
                };
                var result = await UserManager.CreateAsync(student, studentView.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRoles(student.Id, "student", "active");

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

                return PartialView("CoursesList", courses);
            }

            return new ContentResult {Content = "<p>There are no such courses.</p>"};
        }

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
                    return new ContentResult {Content = "There are no such courses.</p>"};
                }

                return PartialView("CoursesList", courses);
            }

            return new ContentResult {Content = "<p>There are no such courses.</p>"};
        }

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

        [HttpGet]
        public ActionResult Edit()
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
            return RedirectToAction("Login", new {area="", controller="Account"});
        }

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

                    try
                    {
                        _studentRepository.Update(student);
                    }
                    catch(Exception ex)
                    {
                        _logger.Error(ex, "Edit (POST) method of controller Student: Error edit a user.");
                    }

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

        public ActionResult LecturerDetails(int id)
        {
            Course course = _courseRepository.GetWithLecturer(id);
            if (course != null)
            {
                return View(course.Lecturer);
            }
            _logger.Warn("LecturerDetails method of controller Student: Course wasn`t found. User received HttpNotFound.");
            return HttpNotFound();
        }

        protected override void Dispose(bool d)
        {
            _courseRepository.Dispose();
            _studentRepository.Dispose();
            _registerRepository.Dispose();
            base.Dispose(d);
        }
    }
}