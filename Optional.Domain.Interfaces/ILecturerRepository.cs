using System.Collections.Generic;
using Optional.Domain.Core;

namespace Optional.Domain.Interfaces
{
    public interface ILecturerRepository
    {
        IEnumerable<Student> GetAll();
        Student Get(string userName);
    }
}