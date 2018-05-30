using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Infrastructure.Data
{
    public class StudentRepository:IStudentRepository
    {
        //private readonly OptionalContext _db;

        //public StudentRepository()
        //{
        //    _db=new OptionalContext();
        //}

        //public void Dispose()
        //{
        //    _db.Dispose();
        //}

        //public IEnumerable<Student> GetAll()
        //{
        //    return _db.Students.ToList();
        //}

        //public Student Get(int id)
        //{
        //    return _db.Students.Find(id);
        //}

        //public IEnumerable<Student> Find(Func<Student, bool> predicate)
        //{
        //    return _db.Students.Where(predicate).ToList();
        //}

        //public void Create(Student item)
        //{
        //    _db.Students.Add(item);
        //    _db.SaveChanges();
        //}

        //public void Update(Student item)
        //{
        //    _db.Entry(item).State = EntityState.Modified;
        //    _db.SaveChanges();
        //}

        //public void Delete(int id)
        //{
        //    Student student = _db.Students.Find(id);
        //    if (student != null)
        //        _db.Students.Remove(student);
        //    _db.SaveChanges();
        //}
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetAll()
        {
            throw new NotImplementedException();
        }

        public Student Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> Find(Func<Student, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Create(Student item)
        {
            throw new NotImplementedException();
        }

        public void Update(Student item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
