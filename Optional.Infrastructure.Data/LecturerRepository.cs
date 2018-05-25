using System;
using System.Collections.Generic;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Infrastructure.Data
{
    public class LecturerRepository:IRepository<Lecturer>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lecturer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Lecturer Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lecturer> Find(Func<Lecturer, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Create(Lecturer item)
        {
            throw new NotImplementedException();
        }

        public void Update(Lecturer item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
