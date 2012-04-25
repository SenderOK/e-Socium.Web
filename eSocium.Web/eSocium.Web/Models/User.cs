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
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "email")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Роль")]
        [Required]
        public int Role { get; set; }
    }
}