using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace eSocium.Web.Models.OpenQuestions.DAL
{
    /// <summary>
    /// Реализует интерфейс DbContext и описывает базу данных для опросов
    /// </summary>
    public class OpenContext : DbContext
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Question>()
                        .HasRequired(q => q.Poll)
                        .WithMany(p => p.Questions)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Answer>()
                        .HasRequired(a => a.Question)
                        .WithMany(q => q.Answers)
                        .WillCascadeOnDelete();
        }
    }
}