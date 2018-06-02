using System;
using System.Collections.Generic;
using Optional.Domain.Core;

namespace Optional.Domain.Interfaces
{
    public interface IStudentRepository:IDisposable
    {
        IEnumerable<Student> GetAll();
        Student Get(string userName);
        IEnumerable<Student> Find(Func<Student, bool> predicate);
        void Update(Student item);
        Student GetWithRegisters(string userName);
    }
}
