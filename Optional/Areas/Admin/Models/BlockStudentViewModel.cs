using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Admin.Models
{
    public class BlockStudentViewModel
    {
        [Display(Name ="User Name")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Group")]
        public string Group { get; set; }

        [Display(Name = "Year of study")]
        public int YearOfStudy { get; set; }

        public bool Blocked { get; set; }
    }
}