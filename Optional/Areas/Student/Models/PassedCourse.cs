using System;
using System.Web.Mvc;

namespace Optional.Areas.Student.Models
{
    public class PassedCourse
    {
        [HiddenInput(DisplayValue = false)]
        public int CourseId { get; set; }
        public string Theme { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public int? Mark { get; set; }
        public Domain.Core.Lecturer Lecturer { get; set; }
    }
}