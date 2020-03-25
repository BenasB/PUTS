using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PUTSWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PUTSWeb.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ProblemDbContext dbContext;

        public HomeController(ProblemDbContext problemDbContext)
        {
            dbContext = problemDbContext;
        }

        public IActionResult Index()
        {
            List<Blog> blogs = dbContext.Blogs.OrderByDescending(m => m.AddedDate).ToList();
            return View(blogs);
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
