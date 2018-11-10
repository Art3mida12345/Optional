using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional.Controllers;
using Optional.Domain.Interfaces;

namespace Optional.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexReturnCurrentViewBagIfParamNull()
        {
            var mock = new Mock<ICourseRepository>();
            HomeController controller = new HomeController(mock.Object);
            string expected1 = "name_desc";
            string expected2 = "Duration";

            ViewResult result = controller.Index(null,null) as ViewResult;
            string actual1 = result.ViewBag.TitleSortParm as string;
            string actual2 = result.ViewBag.DurationSortParm as string;

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        [TestMethod]
        public void IndexReturnCurrentViewBagIfParamIsDesc()
        {
            var mock = new Mock<ICourseRepository>();
            HomeController controller = new HomeController(mock.Object);
            string expected1 = "";

            ViewResult result = controller.Index("name_desc", null) as ViewResult;
            string actual1 = result.ViewBag.TitleSortParm as string;

            Assert.AreEqual(expected1, actual1);
        }

        [TestMethod]
        public void IndexReturnHttpNotFoundIfArgumentNullExceptionOccur()
        {
            var mock = new Mock<ICourseRepository>();
            HomeController controller = new HomeController(mock.Object);
            mock.Setup(c => c.GetAllWithLecturerAndStudents()).Throws<ArgumentNullException>();

            ActionResult result = controller.Index(null, "sometext");

            Assert.AreEqual(result.GetType(), typeof(HttpNotFoundResult));
        }
    }
}
