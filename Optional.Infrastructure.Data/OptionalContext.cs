using System.Data.Entity;
using Optional.Domain.Core;

namespace Optional.Infrastructure.Data
{
    public class OptionalContext:DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
