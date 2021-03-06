﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NLog;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Controllers
{
    /// <summary>
    /// HomeController provides methods and views for all type of user
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor of HomeController
        /// </summary>
        /// <param name="course">Sets repository</param>
        public HomeController(ICourseRepository course)
        {
            _courseRepository = course;
        }

        /// <summary>
        /// Index method of HomeController views info about courses.
        /// </summary>
        /// <param name="sortOrder">The sort value for courses.</param>
        /// <param name="searchString">The value for searching.</param>
        /// <returns>Returns ActionResult HttpNotFoundResult (if some error occur)
        /// or view of courses.</returns>
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DurationSortParm = sortOrder == "Duration" ? "duration_desc" : "Duration";
            ViewBag.AmountOfStudentsSortParm = sortOrder == "Amount" ? "amount_desc" : "Amount";
            try
            {
                var courses = _courseRepository.GetAllWithLecturerAndStudents();
                if (!String.IsNullOrEmpty(searchString))
                {
                    var result = new List<Course>();
                    foreach (var c in courses)
                    {
                        var lecturer = c.Lecturer;
                        if (lecturer != null)
                        {
                            var name = c.Lecturer.LastName + " " 
                                + c.Lecturer.FirstName + " " + c.Lecturer.MiddleName;

                            if (name.Contains(searchString) ||
                                c.Theme.Contains(searchString))
                            {
                                result.Add(c);
                            }
                        }
                        else
                        {
                            if (c.Theme.Contains(searchString))
                                result.Add(c);
                        }
                    }

                    courses = result;
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        courses = courses.OrderByDescending(c => c.Title);
                        break;
                    case "Duration":
                        courses = courses.OrderBy(c => c.Duration);
                        break;
                    case "duration_desc":
                        courses = courses.OrderByDescending(c => c.Duration);
                        break;
                    case "Amount":
                        courses = courses.OrderBy(c => c.Students.Count);
                        break;
                    case "amount_desc":
                        courses = courses.OrderByDescending(c => c.Students.Count);
                        break;
                    default:
                        courses = courses.OrderBy(c => c.Title);
                        break;
                }

                return View(courses.ToList());
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex,
                    "Error occured in Home controller Index Action. Home Page doesn`t work, " +
                    "user redirect to httpnotfound page.");
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Dispose method. Call dispose for _courseRepository.
        /// </summary>
        /// <param name="d"></param>
        protected override void Dispose(bool d)
        {
            _courseRepository.Dispose();
            base.Dispose(d);
        }
    }
}