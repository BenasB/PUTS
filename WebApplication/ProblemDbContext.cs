using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication
{
    public class ProblemDbContext : DbContext
    {
        public DbSet<Problem> Problems { get; set; }

        public ProblemDbContext(DbContextOptions<ProblemDbContext> options) : base(options) { }
    }
}
