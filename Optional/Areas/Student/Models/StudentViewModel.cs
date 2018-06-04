﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Areas.Student.Models
{
    public class StudentViewModel
    {
        [Required]
        [MaxLength(30)]
        [RegularExpression("[A-Za-z0-9_]*", ErrorMessage = "Login can only contain letters of the Latin alphabet, numbers and underscores.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords don`t match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        [Required]
        public string Group { get; set; }

        [Required]
        [RegularExpression("[1-5]",ErrorMessage = "The year of study must be a positive number from one to five.")]
        public int YearOfStudy { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}