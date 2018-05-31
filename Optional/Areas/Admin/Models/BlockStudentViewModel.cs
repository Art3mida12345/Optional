namespace Optional.Areas.Admin.Models
{
    public class BlockStudentViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Group { get; set; }
        public int YearOfStudy { get; set; }
        public bool Blocked { get; set; }
    }
}