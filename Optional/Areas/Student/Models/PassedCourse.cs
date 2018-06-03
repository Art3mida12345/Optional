using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Optional.Areas.Student.Models
{
    public class PassedCourse
    {
        [HiddenInput(DisplayValue = false)]
        public int CourseId { get; set; }
        [Display(Name = "Тема")]
        public string Theme { get; set; }
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Длительность")]
        public int Duration { get; set; }
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Оценка")]
        public int? Mark { get; set; }
    }
}