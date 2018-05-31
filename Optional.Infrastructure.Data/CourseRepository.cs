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
            return _db.Courses.ToList();
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
            Course student = _db.Courses.Find(id);
            if (student != null)
                _db.Courses.Remove(student);
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
    }
}
