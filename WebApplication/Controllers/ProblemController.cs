using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Processing;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        public async Task<IActionResult> Result(string filePath, int id)
        {
            Problem problem = await dbContext.Problems.Include(m => m.Tests).Include(m => m.Examples).FirstOrDefaultAsync(p => p.ProblemID == id);

            if (problem == null)
                return NotFound();

            UserProgram userProgram = new UserProgram();

            var result = userProgram.SetSource(filePath);

            if (result.Status == UserProgram.Result.StatusType.Failed)
                return View("Result", result.Status.ToString() + " " + result.Message);

            result = userProgram.Compile();

            if (result.Status == UserProgram.Result.StatusType.Failed)
                return View("Result", result.Status.ToString() + " " + result.Message);

            int passed = 0;
            foreach (Test t in problem.Tests)
            {
                result = userProgram.Execute(t.GivenInput);

                if (result.Status == UserProgram.Result.StatusType.Successful)
                {
                    result = userProgram.Evaluate(t.ExpectedOutput);

                    if (result.Status == UserProgram.Result.StatusType.Successful)
                        passed++;
                }
            }

            return View("Result", result.Status.ToString() + " " + passed.ToString() + "/" + problem.Tests.Count);
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

        public async Task<IActionResult> Solve(int? id)
        {
            if (id == null)
                return NotFound();

            Problem problem = await dbContext.Problems.Include(m => m.Tests).Include(m => m.Examples).FirstOrDefaultAsync(p => p.ProblemID == id);

            if (problem == null)
                return NotFound();

            return View("Solve", problem);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, int id)
        {
            // TODO: Automatically change extension to the uploaded file's (Don't HARDCODE)
            var filePath = Path.ChangeExtension(Path.GetTempFileName(), ".cpp");

            if (file?.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return await Result(filePath, id);
            }

            return await Solve(id);
        }
    }
}