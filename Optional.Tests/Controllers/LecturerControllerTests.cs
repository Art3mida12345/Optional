using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional.Areas.Lecturer.Controllers;
using Optional.Areas.Lecturer.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Tests.Controllers
{
    [TestClass]
    public class LecturerControllerTests
    {
        [TestMethod]
        public void GradeReturnHttpNotFoundIfExceptionOccur()
        {
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister=new Mock<IRegisterRepository>();
            mockCourse.Setup(c => c.GetWithStudents(1)).Throws(new Exception());
            LecturerController controller = new LecturerController(mockCourse.Object, mockRegister.Object);

            ActionResult result = controller.Grade(1);

            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void GradeReturnContentResultIfStudentsCountZero()
        {
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister = new Mock<IRegisterRepository>();
            mockCourse.Setup(c => c.GetWithStudents(1)).Returns(new Course());
            LecturerController controller = new LecturerController(mockCourse.Object, mockRegister.Object);

            ActionResult result = controller.Grade(1);

            Assert.AreEqual(result.GetType(), typeof(ContentResult));
        }

        [TestMethod]
        public void GradeReturnViewResultIfStudentsCountMoreThanZero()
        {
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister = new Mock<IRegisterRepository>();
            mockCourse.Setup(c => c.GetWithStudents(1)).Returns(new Course(){Students = new List<Student>{new Student()}});
            mockCourse.Setup(r => r.GetMarks(1)).Returns(new List<Register>());
            LecturerController controller = new LecturerController(mockCourse.Object, mockRegister.Object);

            ActionResult result = controller.Grade(1);

            Assert.AreEqual(result.GetType(), typeof(ViewResult));
        }

        [TestMethod]
        public void GradeMarkReturnViewCreateRegisterIfRegisterNull()
        {
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister = new Mock<IRegisterRepository>();
            mockCourse.Setup(r => r.GetMarks(1)).Returns(new List<Register>());
            LecturerController controller = new LecturerController(mockCourse.Object, mockRegister.Object);

            ViewResult result = controller.GradeMark("student2",2);

            Assert.AreEqual(result.ViewName, "CreateRegister");
        }

        [TestMethod]
        public void GradeMarkReturnViewEditRegisterIfRegisterNotNull()
        {
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister = new Mock<IRegisterRepository>();
            mockCourse.Setup(r => r.GetMarks(2)).Returns(new List<Register>(){new Register(){Student = new Student(){UserName = "student2"}}});
            LecturerController controller = new LecturerController(mockCourse.Object, mockRegister.Object);

            ViewResult result = controller.GradeMark("student2", 2);

            Assert.AreEqual(result.ViewName, "EditRegister");
        }

        [TestMethod]
        public void CreateRegisterReturnRedirect()
        {
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister = new Mock<IRegisterRepository>();
            LecturerController controller = new LecturerController(mockCourse.Object, mockRegister.Object);
            mockRegister.Setup(r => r.Create(new Register(), 1, "student")).Verifiable();

            ActionResult result = controller.CreateRegister(new RegisterViewModel());

            Assert.AreEqual(result.GetType(), typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void EditRegisterReturnRedirect()
        {
            var mockCourse = new Mock<ICourseRepository>();
            var mockRegister = new Mock<IRegisterRepository>();
            var register = new Register {Course = new Course {CourseId = 1}, Mark = 5};
            var registerViewModel = new RegisterViewModel {RegisterId = 21, Mark = 5};
            LecturerController controller = new LecturerController(mockCourse.Object, mockRegister.Object);
            mockRegister.Setup(r => r.Get(registerViewModel.RegisterId)).Returns(register).Verifiable();
            mockRegister.Setup(r => r.Update(register));

            RedirectToRouteResult result = controller.EditRegister(registerViewModel) as RedirectToRouteResult;

            Assert.AreEqual(result.GetType(), typeof(RedirectToRouteResult));
            Assert.AreEqual(result.RouteValues["id"], 1);
        }
    }
}
