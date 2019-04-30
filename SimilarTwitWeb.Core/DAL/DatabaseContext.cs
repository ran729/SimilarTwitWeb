using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.DAL
{
    public class DatabaseContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<User> Users { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<Follower>().HasKey(p => p.Id);
            modelBuilder.Entity<Message>().HasKey(p => p.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}
