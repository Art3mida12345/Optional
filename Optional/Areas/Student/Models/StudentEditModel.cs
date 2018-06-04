using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Student.Models
{
    public class StudentEditModel
    {
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Group { get; set; }

        [Required]
        [RegularExpression("[1-5]", ErrorMessage = "The year of study must be a positive number from one to five.")]
        public int YearOfStudy { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}