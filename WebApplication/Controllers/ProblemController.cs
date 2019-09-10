using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            return View(await dbContext.Problems.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind ("Name, Description")]Problem model)
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
                ModelState.AddModelError("", "Unable to save changes");
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