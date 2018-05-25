using System;
using System.Collections.Generic;
using System.Linq;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Infrastructure.Data
{
    public class CourseRepository:IRepository<Course>
    {
        private OptionalContext db;

        public CourseRepository()
        {
            db=new OptionalContext();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Course> GetAll()
        {
            return db.Courses.ToList();
        }

        public Course Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> Find(Func<Course, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Create(Course item)
        {
            throw new NotImplementedException();
        }

        public void Update(Course item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
