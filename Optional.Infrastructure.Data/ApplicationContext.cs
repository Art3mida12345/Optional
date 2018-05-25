using System;
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
    }
}
