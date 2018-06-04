using System.ComponentModel.DataAnnotations;

namespace Optional.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(30)]
        [RegularExpression("[A-Za-z0-9_]*",ErrorMessage = "Login can only contain letters of the Latin alphabet, numbers and underscores.")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}