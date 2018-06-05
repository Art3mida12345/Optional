using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional.Areas.Admin.Controllers;
using Optional.Areas.Admin.Models;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        [TestMethod]
        public void CourseListViewModelNotNull()
        {
            var mock = new Mock<ICourseRepository>();

            mock.Setup(c => c.GetAll()).Returns(new List<Course>());

            AdminController controller = new AdminController(mock.Object);

            ViewResult result = controller.CourseList() as ViewResult;

            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void SelectLecturerToCourseReturnViewWithModel()
        {
            var mock = new Mock<ICourseRepository>();
            mock.Setup(course => course.GetAll()).Returns(new List<Course>());
            AdminController controller = new AdminController(mock.Object);

            ViewResult result = controller.SelectLecturerToCourse("teacher1") as ViewResult;

            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void AddLecturerToCourseRedirectUserToIndexAfterAdd()
        {
            var mock = new Mock<ICourseRepository>();
            string name = "teacher1";
            int id = 1;
            mock.Setup(course => course.AddLecturerToCourse(name,id)).Verifiable();
            AdminController controller = new AdminController(mock.Object);
            RedirectToRouteResult result = controller.AddLecturerToCourse(id,name) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void EditCourseReturnNotFoundIfIdNull()
        {
            var mock = new Mock<ICourseRepository>();
            AdminController controller = new AdminController(mock.Object);
            int? id = null;
            ActionResult result = controller.EditCourse(id) as ActionResult;
            Assert.AreEqual(result.GetType(),typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void EditCourseReturnNotFoundIfExceptionOccur()
        {
            var mock = new Mock<ICourseRepository>();
            int? id = 1;
            mock.Setup(c => c.Get(3)).Throws(new Exception());
            AdminController controller = new AdminController(mock.Object);
            ActionResult result = controller.EditCourse(1);
            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void EditCourseReturnNotFoundResultIfCourseNull()
        {
            var mock = new Mock<ICourseRepository>();
            int? id = 1;
            mock.Setup(c => c.Get(1)).Returns((Course) null);
            AdminController controller = new AdminController(mock.Object);
            ActionResult result = controller.EditCourse(1);
            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void EditCourseReturnCourseViewIfCourseGetSucces()
        {
            var mock = new Mock<ICourseRepository>();
            int? id = 1;
            Course course=new Course();
            mock.Setup(c => c.Get(1)).Returns(course);
            AdminController controller = new AdminController(mock.Object);
            ViewResult result = controller.EditCourse(1) as  ViewResult;
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(result.Model.GetType(), typeof(CourseViewModel));
        }

        [TestMethod]
        public void EditCoursePostReturnModelIfItNotValidAndViewEdit()
        {
            string expected = "";
            var mock = new Mock<ICourseRepository>();
            CourseViewModel course = new CourseViewModel();
            AdminController controller = new AdminController(mock.Object);
            controller.ModelState.AddModelError("Title", "The Title field is required.");
            // act
            ViewResult result = controller.EditCourse(course) as ViewResult;
            // assert
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestMethod]
        public void DeleteCourseGetReturnViewWithModelIfModelValid()
        {
            var mock = new Mock<ICourseRepository>();
            int? id = 1;
            Course course = new Course();
            mock.Setup(c => c.Get(1)).Returns(course);
            AdminController controller = new AdminController(mock.Object);
            ViewResult result = controller.DeleteCourse(1) as ViewResult;
            Assert.AreEqual(result.Model.GetType(), typeof(Course));
        }

        [TestMethod]
        public void DeleteCourseGetReturnHttpNotFoundIfExceptionOcure()
        {
            var mock = new Mock<ICourseRepository>();
            int? id = 1;
            mock.Setup(c => c.Get(1)).Throws(new Exception());
            AdminController controller = new AdminController(mock.Object);
            ActionResult result = controller.DeleteCourse(1);
            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void DeleteCourseGetReturnHttpNotFoundIfCourseNull()
        {
            var mock = new Mock<ICourseRepository>();
            int? id = 1;
            mock.Setup(c => c.Get(1)).Returns((Course) null);
            AdminController controller = new AdminController(mock.Object);
            ActionResult result = controller.DeleteCourse(1);
            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void DeletePostReturnHttpNotFoundIfCourseNull()
        {
            var mock = new Mock<ICourseRepository>();
            mock.Setup(c => c.Get(1)).Returns((Course)null);
            AdminController controller = new AdminController(mock.Object);
            ActionResult result = controller.DeleteCourseConfirmed(31);
            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void DeletePostWorkCurrentAndRedirectToActionCourseList()
        {
            var mock = new Mock<ICourseRepository>();
            mock.Setup(c => c.Get(2)).Returns(new Course());
            AdminController controller = new AdminController(mock.Object);
            RedirectToRouteResult result = controller.DeleteCourseConfirmed(2) as RedirectToRouteResult;
            mock.Setup(m=>m.Delete(2)).Verifiable();
            Assert.AreEqual("CourseList", result.RouteValues["action"]);
        }
    }
}
