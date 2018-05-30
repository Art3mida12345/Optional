using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Optional.Domain.Core
{
    public class Student:IdentityUser
    {
        public Student()
        {
            Courses=new List<Course>();
            Registers=new List<Register>();
        }


        public string Group { get; set; }
        public int YearOfStudy { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Register> Registers { get; set; }
    }
}
