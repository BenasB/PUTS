using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IActionResult> List()
        {
            List<Problem> problemList = await dbContext.Problems.Include(m => m.Tests).ToListAsync();
            return View(problemList);
        }

        public IActionResult Create()
        {
            Problem p = new Problem();
            return View(p);
        }

        public PartialViewResult TestCreator()
        {
            return PartialView("TestCreatorPartial", new Test());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Problem model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.Problems.Add(model);
                    await dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(List));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to create a problem");
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Problem problem = await dbContext.Problems.FindAsync(id);
            if (problem == null)
            {
                return RedirectToAction(nameof(List));
            }

            try
            {
                dbContext.Problems.Remove(problem);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(List));
            }
        }
    }
}