using System;

namespace Optional.Domain.Core
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
    }
}
