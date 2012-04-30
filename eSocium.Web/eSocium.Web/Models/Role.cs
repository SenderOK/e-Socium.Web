using System.ComponentModel.DataAnnotations;

namespace eSocium.Web.Models
{
    /// <summary>
    /// Класс, описывающий роль пользователя
    /// </summary>
    public class Role
    {
        [Key]
        [Required]
        public int RoleId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}