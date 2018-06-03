using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using Optional.Areas.Lecturer.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Areas.Lecturer.Controllers
{
    [Authorize(Roles = "teacher")]
    public class LecturerController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly ICourseRepository _courseRepository;
        private readonly IRegisterRepository _registerRepository;

        public LecturerController(ICourseRepository courseRepository, IRegisterRepository registerRepository)
        {
            _courseRepository = courseRepository;
            _registerRepository = registerRepository;
        }

        public ActionResult Index()
        {
            Domain.Core.Lecturer user = (Domain.Core.Lecturer)UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                return View(user);
            }

            return HttpNotFound();
        }

        public ActionResult CourseList()
        {
            Domain.Core.Lecturer user = (Domain.Core.Lecturer)UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
            {
                var courses = _courseRepository.GetAll().ToList();
                courses = courses.Where(course => course.Lecturer.IfNotNull(l => l.UserName.Equals(user.UserName)))
                    .ToList();
                if (courses.Count!=0) return PartialView(courses);
            }

            return new ContentResult{Content = "<p>Вы не закреплены ни за одним курсом.</p>"};
        }

        public ActionResult Grade(int id)
        {
            var students = _courseRepository.GetWithStudents(id).Students.ToList();

            if (students.Count == 0)
            {
                return new ContentResult{Content = "<p>На курсе не зарегестрировано ни одного студента.</p>"};
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
                    Mark = registers.FirstOrDefault(r => r.Student.UserName==student.UserName)?.Mark,
                    UserName = student.UserName,
                    MiddleName = student.MiddleName,
                    PhoneNumber = student.PhoneNumber,
                    YearOfStudy = student.YearOfStudy
                });
            }
            ViewBag.CourseId = id;
            return View(studentsWithMarks);
        }

        [HttpGet]
        public ViewResult GradeMark(string name, int courseId)
        {
            var register = _courseRepository.GetMarks(courseId).FirstOrDefault(r => r.Student.UserName.Equals(name));
            if (register != null)
            {
                return View("EditRegister", new RegisterViewModel
                {
                    RegisterId = register.RegisterId, Mark = register.Mark
                });
            }

            return View("CreateRegister", new RegisterViewModel
            {
                CourseId = courseId,
                StudentName = name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRegister(RegisterViewModel registerViewModel)
        {
            Register register = new Register
            {
                Mark = registerViewModel.Mark
            };
            _registerRepository.Create(register, registerViewModel.CourseId, registerViewModel.StudentName);
            return RedirectToAction("Grade", new {id=registerViewModel.CourseId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRegister(RegisterViewModel registerViewModel)
        {
            Register register = _registerRepository.Get(registerViewModel.RegisterId);
            register.Mark = registerViewModel.Mark;
            _registerRepository.Update(register);
            return RedirectToAction("Grade", new { id = register.Course.CourseId });
        }

        protected override void Dispose(bool disposing)
        {
            _courseRepository.Dispose();
            _registerRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}