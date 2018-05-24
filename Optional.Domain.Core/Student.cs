using System;
using System.Collections.Generic;

namespace Optional.Domain.Core
{
    public class Student
    {
        public Student()
        {
            Courses=new List<Course>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }
        public int YearOfStudy { get; set; }
        public string Phone { get; set; }
        public ICollection<Course> Courses { get; set; }
        public Register Register { get; set; }
    }
}
