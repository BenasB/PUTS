using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Areas.Identity.Data;
using WebApplication.Helpers;

[assembly: HostingStartup(typeof(WebApplication.Areas.Identity.IdentityHostingStartup))]
namespace WebApplication.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ProblemDbContext>(options =>
                    options.UseMySql(
                        context.Configuration.GetConnectionString("ProblemDatabase")));

                services.AddDefaultIdentity<ApplicationUser>()
                    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                    .AddEntityFrameworkStores<ProblemDbContext>();
            });
        }
    }
}