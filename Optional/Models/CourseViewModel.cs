using System;
using Optional.Domain.Core;

namespace Optional.Models
{
    public class CourseViewModel
    {
        public string Theme { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public Lecturer Lecturer { get; set; }
    }
}