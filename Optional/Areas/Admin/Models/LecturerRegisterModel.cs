using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Admin.Models
{
    public class LecturerRegisterModel
    {
        [Required]
        [MaxLength(30)]
        [RegularExpression("[A-Za-z0-9_]*", ErrorMessage = "Login can only contain letters of the Latin alphabet, numbers and underscores.")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords don`t match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression("(?m)^(\\w-*){1,40}$", ErrorMessage = "Name can only contain letters and dashes")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("(?m)^(\\w-*){1,40}$", ErrorMessage = "Name can only contain letters and dashes")]
        public string MiddleName { get; set; }

        [Required]
        [RegularExpression("(?m)^(\\w-*){1,40}$", ErrorMessage = "Name can only contain letters and dashes")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        public string Department { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}