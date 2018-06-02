using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using Optional.Areas.Lecturer.Models;
using Optional.Domain.Interfaces;
using Optional.Infrastructure.Data;

namespace Optional.Areas.Lecturer.Controllers
{
    [Authorize(Roles = "teacher")]
    public class LecturerController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private readonly ICourseRepository _courseRepository;

        public LecturerController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
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
            var students = _courseRepository.Get(id).Students.ToList();

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
                    Mark = registers.First(r=>r.Student.UserName==student.UserName).Mark,
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
            var register = _courseRepository.GetMarks(courseId).First(r => r.Student.UserName.Equals(name));
            if (register != null)
            {
                return View(register);
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _courseRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}