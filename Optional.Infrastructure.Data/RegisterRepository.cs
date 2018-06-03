using System.Data.Entity;
using System.Linq;
using Optional.Domain.Core;
using Optional.Domain.Interfaces;

namespace Optional.Infrastructure.Data
{
    public class RegisterRepository:IRegisterRepository
    {
        private readonly ApplicationContext _db;

        public RegisterRepository()
        {
            _db=new ApplicationContext();
        }

        public Register Get(int id)
        {
            return _db.Registers.Include(r=>r.Course).First(r=>r.RegisterId==id);
        }

        public void Create(Register item, int courseId, string studentName)
        {
            item.Student = _db.Users.Where(user => user.UserName == studentName).OfType<Student>().First();
            item.Course = _db.Courses.Find(courseId);
            _db.Registers.Add(item);
            _db.SaveChanges();
        }

        public void Update(Register item)
        {
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public int GetMarkOfStudent(int courseId, string userName)
        {
            var result = _db.Registers.Include(r => r.Student).Include(r => r.Course)
                .FirstOrDefault(r => r.Student.UserName.Equals(userName) && r.Course.CourseId == courseId);
            return result?.Mark ?? 0;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
