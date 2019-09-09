using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ProblemController : Controller
    {
        ProblemDbContext dbContext;

        public ProblemController(ProblemDbContext problemDbContext)
        {
            dbContext = problemDbContext;
        }

        public IActionResult List()
        {
            return View(dbContext.Problems.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Problem model)
        {
            if (ModelState.IsValid)
            {
                dbContext.Problems.Add(model);
                dbContext.SaveChanges();

                return RedirectToAction("List");
            }

            return View();
        }
    }
}