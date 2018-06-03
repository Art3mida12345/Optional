using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Optional.Areas.Lecturer.Models
{
    public class RegisterViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int RegisterId { get; set; }

        [Required]
        [Display(Name = "Оценка")]
        [RegularExpression("[1-5]", ErrorMessage = "Оценка должна быть числом в диапазоне от 1 до 5.")]
        public int Mark { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string StudentName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int CourseId { get; set; }
    }
}