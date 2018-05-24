using System.ComponentModel.DataAnnotations.Schema;

namespace Optional.Domain.Core
{
    public class Register
    {
        public int RegisterId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ForeignKey("Student")]
        public int Id;
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
