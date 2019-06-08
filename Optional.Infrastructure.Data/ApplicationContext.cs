using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Optional.Domain.Core;

namespace Optional.Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("OptionalContext")
        {
        }

        static ApplicationContext()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ApplicationContext>());
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Lecturer>().ToTable("Lecturers");
            modelBuilder.Entity<Student>().ToTable("Students");
        }

        public DbSet<Register> Registers { get; set; }
        public DbSet<Course> Courses { get; set; }


    }
}
