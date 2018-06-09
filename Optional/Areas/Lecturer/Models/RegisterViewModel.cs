using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Optional.Areas.Lecturer.Models
{
    public class RegisterViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int RegisterId { get; set; }

        [Required]
        [Display(Name = "Mark")]
        [RegularExpression("[1-5]", ErrorMessage = "The mark must be a number between 1 and 5.")]
        public int Mark { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string StudentName { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int CourseId { get; set; }
    }
}