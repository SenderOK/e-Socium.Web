using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        [Display(Name = "Роль")]
        public string Name { get; set; }
    }
}