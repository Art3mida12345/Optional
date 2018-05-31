using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Admin.Models
{
    public class LecturerRegisterModel
    {
        [Required]
        [MaxLength(30)]
        [RegularExpression("[A-Za-z0-9_]*", ErrorMessage = "Логин может содержать буквы латинского алфавита.")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [RegularExpression("[А-ЯЁа-яё]{1,40}")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("[А-ЯЁа-яё]{1,40}")]
        public string MiddleName { get; set; }

        [Required]
        [RegularExpression("[А-ЯЁа-яё]{1,40}")]
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
    }
}