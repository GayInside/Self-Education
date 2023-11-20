using AuthVideo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AuthVideo.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Content> Contents { get; set; }

        public ApplicationContext()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>().Property(p => p.Name).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=VideoAuthDataBase;Username=postgres;Password=GAAgaa2005");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
