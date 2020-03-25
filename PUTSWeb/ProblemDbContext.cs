using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PUTSWeb.Areas.Identity.Data;
using PUTSWeb.Models;

namespace PUTSWeb
{
    public class ProblemDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<Blog> Blogs { get; set; }

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
