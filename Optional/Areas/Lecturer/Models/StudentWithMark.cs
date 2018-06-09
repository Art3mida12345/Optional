using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Lecturer.Models
{
    public class StudentWithMark
    {
        [Display(Name = "Mark")]
        public int? Mark { get; set; }
        [Display(Name = "Group")]
        public string Group { get; set; }
        [Display(Name = "Year")]
        public int YearOfStudy { get; set; }
        [Display(Name = "Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Login")]
        public string UserName { get; set; }
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}