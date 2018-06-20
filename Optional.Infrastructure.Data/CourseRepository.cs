using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Infrastructure.Data
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationContext _db;

        public CourseRepository()
        {
            _db=new ApplicationContext();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public IEnumerable<Course> GetAll()
        {
            return _db.Courses.Include(c => c.Lecturer).ToList();
        }

        public IEnumerable<Course> GetAllWithLecturerAndStudents()
        {
            return _db.Courses.Include(c => c.Lecturer).Include(c => c.Students).ToList();
        }

        public Course GetWithStudents(int id)
        {
            return _db.Courses.Include(c=>c.Students).First(c => c.CourseId==id);
        }

        public Course Get(int id)
        {
            return _db.Courses.Find(id);
        }

        public IEnumerable<Course> Find(Func<Course, bool> predicate)
        {
            return _db.Courses.Where(predicate).ToList();
        }

        public void Create(Course item)
        {
            _db.Courses.Add(item);
            _db.SaveChanges();
        }

        public void Update(Course item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var registers = _db.Registers.Include(r=>r.Course).Where(r => r.Course.CourseId == id).ToArray();

            if (registers.Length != 0)
            {
                _db.Registers.RemoveRange(registers);
            }

            //var students = _db.Users.OfType<Student>().Include(s => s.Courses).ToList();
            //foreach (var student in students)
            //{
            //    var course = student.Courses.FirstOrDefault(c => c.CourseId == id);
            //    if (course != null)
            //    {
            //        student.Courses.Remove(course);
            //    }
            //}
            _db.Entry(_db.Courses.Find(id)).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public void AddLecturerToCourse(string lecturer, int id)
        {
            var course = _db.Courses.Find(id);
            if (course != null)
            {
                course.Lecturer = (Lecturer)_db.Users.First(u => u.UserName==lecturer);
                _db.SaveChanges();
            }
        }

        public void AddStudentToCourse(string studentName, int courseId)
        {
            var course = _db.Courses.Where(c=>c.CourseId==courseId).Include(c=>c.Students).First();
            var student = (Student) _db.Users.First(u => u.UserName == studentName);
            if (course != null && !course.Students.Contains(student))
            {
                course.Students.Add((Student)_db.Users.First(u=>u.UserName==studentName));
                _db.SaveChanges();
            }
        }

        public IEnumerable<Register> GetMarks(int courseId)
        {
            return _db.Registers.Include(r=>r.Student).Where(r => r.Course.CourseId == courseId).ToList();
        }

        public Course GetWithLecturer(int courseId)
        {
            return _db.Courses.Include(c => c.Lecturer).FirstOrDefault(c => c.CourseId == courseId);
        }
    }
}
