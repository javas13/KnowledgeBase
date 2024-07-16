using Microsoft.EntityFrameworkCore;
using StudyTodocups.Models;

namespace StudyTodocups.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<PossibleAnswer> PossibleAnswers { get; set; }
         public DbSet<Module> Modules { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<TestTry> TestTries { get; set; }
    }
}
