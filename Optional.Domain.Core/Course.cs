﻿using System;
using System.Collections.Generic;

namespace Optional.Domain.Core
{
    public class Course
    {
        public Course()
        {
            Students=new List<Student>();
        }

        public int CourseId { get; set; }
        public string Theme { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public int? LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
