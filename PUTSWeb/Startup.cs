using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PUTSWeb.Areas.Identity.Data;
using PUTSWeb.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace PUTSWeb
{
  public class Startup
  {
    IHostingEnvironment environment;

    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
      Configuration = configuration;
      environment = env;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<CookiePolicyOptions>(options =>
      {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

      var keysFolder = Path.Combine(environment.ContentRootPath, "Keys");
      services.AddDataProtection()
          .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
          .SetApplicationName("PUTS");

      services.AddLocalization(options => options.ResourcesPath = "Resources");
      services.AddDbContext<ProblemDbContext>(options => options.UseMySql(Configuration.GetConnectionString("ProblemDatabase")));
      services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();

      services.AddMvc(config =>
      {
        var policy = new AuthorizationPolicyBuilder()
                               .RequireAuthenticatedUser()
                               .Build();
        config.Filters.Add(new AuthorizeFilter(policy));
      })
          .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
          .AddViewLocalization()
          .AddDataAnnotationsLocalization();

      services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      UpdateDatabase(app);

      if (environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      var supportedCultures = new[]
      {
                new CultureInfo("en-US"),
                new CultureInfo("lt-LT")
            };

      app.UseRequestLocalization(new RequestLocalizationOptions
      {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
      });

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseAuthentication();
      app.UseCookiePolicy();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });

      CreateRoles(serviceProvider).Wait();
    }

    private static void UpdateDatabase(IApplicationBuilder app)
    {
      using (var serviceScope = app.ApplicationServices
          .GetRequiredService<IServiceScopeFactory>()
          .CreateScope())
      {
        using (var context = serviceScope.ServiceProvider.GetService<ProblemDbContext>())
        {
          context.Database.Migrate();
        }
      }
    }

    private async Task CreateRoles(IServiceProvider serviceProvider)
    {
      var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      string[] roleNames = { "SuperUser", "Admin", "Member", "Moderator" };
      IdentityResult roleResult;

      foreach (var roleName in roleNames)
      {
        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
          roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
      }

      var superUser = new ApplicationUser
      {
        UserName = Configuration["SuperUser:Username"],
      };

      string superUserPassword = Configuration["SuperUser:Password"];
      var tempUser = await UserManager.FindByNameAsync(superUser.UserName);

      if (tempUser == null)
      {
        var createPowerUser = await UserManager.CreateAsync(superUser, superUserPassword);
        if (createPowerUser.Succeeded)
        {
          await UserManager.AddToRoleAsync(superUser, "Admin");
          await UserManager.AddToRoleAsync(superUser, "SuperUser");
        }
      }
    }
  }
}