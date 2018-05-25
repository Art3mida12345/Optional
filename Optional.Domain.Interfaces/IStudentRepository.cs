using System;
using System.Collections.Generic;
using Optional.Domain.Core;

namespace Optional.Domain.Interfaces
{
    public interface IStudentRepository:IDisposable
    {
        IEnumerable<Student> GetAll();
        Student Get(int id);
        IEnumerable<Student> Find(Func<Student, Boolean> predicate);
        void Create(Student item);
        void Update(Student item);
        void Delete(int id);
    }
}
