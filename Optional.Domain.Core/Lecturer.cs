using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Optional.Domain.Core
{
    public class Lecturer:IdentityUser
    {
        public string Department { get; set; }
    }
}
