namespace Optional.Domain.Core
{
    public class Register
    {
        public int RegisterId { get; set; }
        public int Mark { get; set; }
        public int? StudentId { get; set; }
        public Student Student { get; set; }
    }
}
