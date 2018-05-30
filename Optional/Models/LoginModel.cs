using System.ComponentModel.DataAnnotations;

namespace Optional.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(30)]
        [RegularExpression("[A-Za-z0-9_]*",ErrorMessage = "Логин может содержать буквы латинского алфавита.")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}