using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Admin.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required]
        public string Theme { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Duration { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public string LecturerName { get; set; }
    }
}