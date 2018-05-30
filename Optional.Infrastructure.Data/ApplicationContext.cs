using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Optional.Domain.Core;

namespace Optional.Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("OptionalContext") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
