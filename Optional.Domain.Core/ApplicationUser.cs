using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Optional.Domain.Core
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-18);
        public string Gender { get; set; }
        public string Phone { get; set; }
    }
}
