using System.ComponentModel.DataAnnotations;

namespace eSocium.Web.Models
{
    /// <summary>
    /// Класс, который описывает пользователя системы
    /// </summary>
    public class User
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        [Display(Name = "email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Role")]
        public int Role { get; set; }
    }
}