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
            List<Problem> problemList = await dbContext.Problems.Include(m => m.Tests).Include(m => m.Examples).ToListAsync();
            return View(problemList);
        }

        public IActionResult Create()
        {
            Problem p = new Problem();
            return View(p);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Problem problem = await dbContext.Problems.Include(m => m.Tests).Include(m => m.Examples).FirstOrDefaultAsync(p => p.ProblemID == id);

            if (problem == null)
                return NotFound();

            return View(problem);
        }

        public PartialViewResult TestCreator()
        {
            return PartialView("TestCreator", new Test());
        }

        public PartialViewResult ExampleCreator()
        {
            return PartialView("ExampleCreator", new Example());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Problem problem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbContext.Problems.Add(problem);
                    await dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(List));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to create a problem");
            }

            return View(problem);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProblem(int? id)
        {
            if (id == null)
                return NotFound();

            Problem problemToEdit = await dbContext.Problems.Include(m => m.Tests).Include(m => m.Examples).FirstOrDefaultAsync(p => p.ProblemID == id);

            if (await TryUpdateModelAsync(problemToEdit))
            {
                try
                {
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(List));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. ");
                    return View(problemToEdit);
                }               
            }

            return View(problemToEdit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Problem problem = await dbContext.Problems.Include(m => m.Tests).Include(m => m.Examples).FirstOrDefaultAsync(m => m.ProblemID == id);
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