using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace eSocium.Web.Models.DAL
{
    /// <summary>
    /// Собственный инициализатор базы данных
    /// В случае, если модель поменялась, то пересоздать базу и добавить в нее некоторые компоненты
    /// </summary>
    public class UserInitializer : DropCreateDatabaseIfModelChanges<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            var users = new List<User>
            {
                new User { UserId = 1, Email = "admin@admin.com", Login = "admin", Password = PasswordHash.ComputeHash("admin"), Role = 1 }
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

            var roles = new List<Role>
            {
                new Role { RoleId = 1, Name = "Администратор" },
                new Role { RoleId = 2, Name = "Пользователь" }
            };
            roles.ForEach(s => context.Roles.Add(s));
            context.SaveChanges();
        }
    }
}