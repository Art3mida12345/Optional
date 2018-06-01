using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Student.Models
{
    public class StudentViewModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Логин")]
        [RegularExpression("[A-Za-z0-9_]*", ErrorMessage = "Логин может содержать только буквы латинского алфавита, цифры и знаки подчеркивания.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Группа")]
        public string Group { get; set; }

        [Required]
        [Display(Name = "Год обучения")]
        [RegularExpression("[1-5]",ErrorMessage = "Год обучения должен быть положительным числом от одного до 5")]
        public int YearOfStudy { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}