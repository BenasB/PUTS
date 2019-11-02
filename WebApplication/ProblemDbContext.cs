using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication
{
    public class ProblemDbContext : DbContext
    {
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Example> Example { get; set; }

        public ProblemDbContext(DbContextOptions<ProblemDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Problem>()
                .HasMany(p => p.Tests)
                .WithOne(t => t.Problem)
                .IsRequired();

            modelBuilder.Entity<Problem>()
                .HasMany(p => p.Examples)
                .WithOne(e => e.Problem)
                .IsRequired();
        }       
    }
}
