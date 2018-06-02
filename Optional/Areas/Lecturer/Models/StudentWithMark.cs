using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Lecturer.Models
{
    public class StudentWithMark
    {
        [Display(Name = "Оценка")]
        public int? Mark { get; set; }
        [Display(Name = "Группа")]
        public string Group { get; set; }
        [Display(Name = "Год обучения")]
        public int YearOfStudy { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Пол")]
        public string Gender { get; set; }
        [Display(Name = "Логин")]
        public string UserName { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}