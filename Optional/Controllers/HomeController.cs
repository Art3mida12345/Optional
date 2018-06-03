using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        //private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public HomeController(ICourseRepository course)
        {
            _courseRepository = course;
        }

        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DurationSortParm = sortOrder == "Duration" ? "duration_desc" : "Duration";
            var courses = _courseRepository.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                var result = new List<Course>();
                foreach (var c in courses)
                {
                    var lecturer = c.Lecturer;
                    if(lecturer!=null)
                    {
                        if (c.Lecturer.LastName.Contains(searchString)
                            || c.Lecturer.FirstName.Contains(searchString) ||
                            c.Lecturer.MiddleName.Contains(searchString) ||
                            c.Theme.Contains(searchString))
                        {
                            result.Add(c);
                        }
                    }
                    else
                    {
                        if(c.Theme.Contains(searchString))
                            result.Add(c);
                    }
                }

                courses = result;
            }

            switch (sortOrder)
            {
                case "name_desc":
                    courses = courses.OrderByDescending(c=>c.Title);
                    break;
                case "Duration":
                    courses = courses.OrderBy(c=>c.Duration);
                    break;
                case "duration_desc":
                    courses = courses.OrderByDescending(c=>c.Duration);
                    break;
                default:
                    courses = courses.OrderBy(c=>c.Title);
                    break;
            }
            return View(courses.ToList());
            //return View(_courseRepository.GetAll().ToList());
        }

        protected override void Dispose(bool d)
        {
            _courseRepository.Dispose();
            base.Dispose(d);
        }
    }
}