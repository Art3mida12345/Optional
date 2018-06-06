using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional.Areas.Student.Controllers;
using Optional.Areas.Student.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTests
    {
        [TestMethod]
        public void LecturerDetailsReturnHttpNotFoundIfCourseNull()
        {
            var mockStudent = new Mock<IStudentRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister = new Mock<IRegisterRepository>();
            StudentController controller =
                new StudentController(mockCourse.Object, mockStudent.Object, mockRegister.Object);
            ActionResult result = controller.LecturerDetails(1);

            mockCourse.Setup(c => c.GetWithLecturer(1)).Returns((Course) null);

            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void LecturerDetailsReturnHttpNotFoundIfExceptionOccure()
        {
            var mockStudent = new Mock<IStudentRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister = new Mock<IRegisterRepository>();
            StudentController controller =
                new StudentController(mockCourse.Object, mockStudent.Object, mockRegister.Object);
            ActionResult result = controller.LecturerDetails(1);

            mockCourse.Setup(c => c.GetWithLecturer(1)).Throws<Exception>();

            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void LecturerDetailsReturnViewWithModelIfCourseFoundSuccessfully()
        {
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            mockCourse.Setup(c => c.GetWithLecturer(2))
                .Returns(new Course {Lecturer = new Lecturer {UserName = "teacher2"}}).Verifiable();

            ViewResult result = controller.LecturerDetails(2) as ViewResult;


            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void NotStartedCourseReturnsContextResultIfStudentNull()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns((Student) null).Verifiable();

            ContentResult result = controller.NotStartedCourses() as ContentResult;

            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void NotStartedCoursesReturnsPartialViewWithModelIfCountCourseMoreThanZero()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            var student = new Student();
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns(student);
            student.Courses = new List<Course> {new Course {StartDate = DateTime.Now.AddDays(2)}};

            PartialViewResult result = controller.NotStartedCourses() as PartialViewResult;


            Assert.AreEqual(result.GetType(), typeof(PartialViewResult));
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void NotStatedCoursesReturnContentResultIfCourseCountZero()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            var student = new Student();
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns(student);
            student.Courses = new List<Course> {new Course {StartDate = DateTime.Now}};

            ContentResult result = controller.NotStartedCourses() as ContentResult;


            Assert.AreEqual(result.GetType(), typeof(ContentResult));
        }

        [TestMethod]
        public void StartedCourseReturnsContextResultIfStudentNull()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns((Student) null).Verifiable();

            ContentResult result = controller.StartedCourses() as ContentResult;

            Assert.AreEqual(result.GetType(), typeof(ContentResult));
        }

        [TestMethod]
        public void StartedCoursesReturnsPartialViewWithModelIfCountCourseMoreThanZero()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            var student = new Student();
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns(student);
            student.Courses = new List<Course> {new Course {StartDate = DateTime.Now.AddDays(-5), Duration = 9}};

            PartialViewResult result = controller.StartedCourses() as PartialViewResult;


            Assert.AreEqual(result.GetType(), typeof(PartialViewResult));
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void StatedCoursesReturnContentResultIfCourseCountZero()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            var student = new Student();
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns(student);
            student.Courses = new List<Course> {new Course {StartDate = DateTime.Now.AddDays(1)}};

            ContentResult result = controller.StartedCourses() as ContentResult;


            Assert.AreEqual(result.GetType(), typeof(ContentResult));
        }

        [TestMethod]
        public void PassedCourseReturnsContextResultIfStudentNull()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns((Student) null).Verifiable();

            ContentResult result = controller.PassedCourses() as ContentResult;

            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void PassedCoursesReturnsPartialViewWithModelIfCountCourseMoreThanZero()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            var student = new Student();
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns(student);
            student.Courses = new List<Course>
            {
                new Course {StartDate = DateTime.Now.AddDays(-5), Duration = 1, CourseId = 1}
            };
            mockCourse.Setup(c => c.GetWithLecturer(student.Courses.First().CourseId))
                .Returns(new Course() {Lecturer = new Lecturer()});

            PartialViewResult result = controller.PassedCourses() as PartialViewResult;


            Assert.AreEqual(result.GetType(), typeof(PartialViewResult));
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void PassedCoursesReturnContentResultIfCourseCountZero()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            var student = new Student();
            mockStudent.Setup(c => c.GetWithCourses(identity.Name)).Returns(student);
            student.Courses = new List<Course> {new Course {StartDate = DateTime.Now, Duration = 4}};

            ContentResult result = controller.PassedCourses() as ContentResult;


            Assert.AreEqual(result.GetType(), typeof(ContentResult));
        }

        [TestMethod]
        public void EditReturnHttpNotFoundIfStudentNull()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            mockStudent.Setup(c => c.Get(identity.Name)).Returns((Student) null);

            ActionResult result = controller.Edit();

            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void EditGetReturnHttpNotFoundIfExceptionOccur()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            mockStudent.Setup(c => c.Get(identity.Name)).Throws<Exception>();

            ActionResult result = controller.Edit();

            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void EditPostReturnViewResultIfStudentNull()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            mockStudent.Setup(c => c.Get(identity.Name)).Returns((Student) null);

            ActionResult result = controller.Edit(new StudentEditModel());

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [TestMethod]
        public void EditPostReturnViewResultIfExceptionOccur()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            mockStudent.Setup(c => c.Get(identity.Name)).Throws<Exception>();

            ActionResult result = controller.Edit(new StudentEditModel());

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [TestMethod]
        public void EditPostReturnRedirectIfUpdateOk()
        {
            var identity = new GenericIdentity("student1");
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("student1");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            var mockStudent = new Mock<IStudentRepository>();
            var mock = new Mock<IRegisterRepository>();
            var mockCourse = new Mock<ICourseRepository>();
            StudentController controller = new StudentController(mockCourse.Object, mockStudent.Object, mock.Object);
            controller.ControllerContext = controllerContext.Object;
            mockStudent.Setup(c => c.Get(identity.Name)).Returns(new Student());
            mockStudent.Setup(c=>c.Update(new Student())).Verifiable();

            ActionResult result = controller.Edit(new StudentEditModel());

            Assert.AreEqual(result.GetType(), typeof(RedirectToRouteResult));
        }
    }
}
