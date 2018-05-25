using System;
using System.Collections.Generic;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Infrastructure.Data
{
    public class RegisterRepository:IRepository<Register>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Register> GetAll()
        {
            throw new NotImplementedException();
        }

        public Register Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Register> Find(Func<Register, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Create(Register item)
        {
            throw new NotImplementedException();
        }

        public void Update(Register item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
