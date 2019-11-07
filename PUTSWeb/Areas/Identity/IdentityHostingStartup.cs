using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PUTSWeb.Areas.Identity.Data;

[assembly: HostingStartup(typeof(PUTSWeb.Areas.Identity.IdentityHostingStartup))]
namespace PUTSWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ProblemDbContext>(options =>
                    options.UseMySql(
                        context.Configuration.GetConnectionString("ProblemDatabase")));

                // Configure the identity using the old-style API, ASP.NET Corre 2. bug
                // See https://github.com/aspnet/Identity/issues/1813
                /*services.AddDefaultIdentity<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                    .AddEntityFrameworkStores<ProblemDbContext>();*/

                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders()
                    .AddEntityFrameworkStores<ProblemDbContext>();
            });
        }
    }
}