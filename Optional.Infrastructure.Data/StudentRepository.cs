﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Infrastructure.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationContext _db;

        public StudentRepository()
        {
            _db = new ApplicationContext();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public IEnumerable<Student> GetAll()
        {
            return _db.Users.OfType<Student>().Include(s=>s.Courses).ToList();
        }

        public Student Get(string userName)
        {
            return _db.Users.OfType<Student>().FirstOrDefault(s=>s.UserName==userName);
        }

        public IEnumerable<Student> Find(Func<Student, bool> predicate)
        {
            return _db.Users.OfType<Student>().AsEnumerable().Where(predicate).ToList();
        }

        public void Update(Student item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public Student GetWithRegisters(string userName)
        {
            return _db.Users.OfType<Student>().Include(s => s.Registers).First(u => u.UserName.Equals(userName));
        }

        public Student GetWithCourses(string userName)
        {
            return _db.Users.OfType<Student>().Include(s => s.Courses).FirstOrDefault(s => s.UserName == userName);
        }
    }
}
