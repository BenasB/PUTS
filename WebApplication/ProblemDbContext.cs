using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Areas.Identity.Data;
using WebApplication.Models;

namespace WebApplication
{
    public class ProblemDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Example> Example { get; set; }

        public ProblemDbContext(DbContextOptions<ProblemDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
