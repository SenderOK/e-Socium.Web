using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace eSocium.Web.Models.DAL
{
    /// <summary>
    /// Реализует интерфейс DbContext и описывает базу данных
    /// </summary>
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}