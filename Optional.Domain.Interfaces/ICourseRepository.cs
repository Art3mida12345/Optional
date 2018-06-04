using System;
using System.Collections.Generic;
using Optional.Domain.Core;

namespace Optional.Domain.Interfaces
{
    public interface ICourseRepository:IDisposable
    {
        IEnumerable<Course> GetAll();
        Course Get(int id);
        IEnumerable<Course> Find(Func<Course, Boolean> predicate);
        void Create(Course item);
        void Update(Course item);
        void Delete(int id);
        void AddLecturerToCourse(string lecturer, int courseId);
        void AddStudentToCourse(string student, int coirseId);
        IEnumerable<Register> GetMarks(int courseId);
        Course GetWithStudents(int id);
        Course GetWithLecturer(int courseId);
    }
}
