using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Processing;
using System;
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
        readonly IHostingEnvironment hostingEnvironment; 

        public ProblemController(ProblemDbContext problemDbContext, IHostingEnvironment hostingEnv)
        {
            dbContext = problemDbContext;
            hostingEnvironment = hostingEnv;
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

        public async Task<IActionResult> Load(int id, string filePath)
        {
            Problem problem = await dbContext.Problems.Include(m => m.Tests).FirstOrDefaultAsync(p => p.ProblemID == id);

            if (problem == null || !System.IO.File.Exists(filePath))
                return NotFound();

            return View();
        }

        public async Task<IActionResult> Result(int id, string filePath)
        {
            Problem problem = await dbContext.Problems.Include(m => m.Tests).FirstOrDefaultAsync(p => p.ProblemID == id);

            if (problem == null)
                return RedirectToAction(nameof(List));

            if (!System.IO.File.Exists(filePath))
                return RedirectToAction(nameof(Solve), new { id });

            string parentFolderPath = Directory.GetParent(filePath).FullName;
            ResultViewModel viewModel = new ResultViewModel()
            {
                ProblemID = id,
                Name = problem.Name,
            };
            UserProgram userProgram = new UserProgram();

            var result = userProgram.SetSource(filePath);
            viewModel.CompilationResult = result;

            if (result.Status == UserProgram.Result.StatusType.Failed)
            {
                Directory.Delete(parentFolderPath, true);
                return View(viewModel);
            }

            result = userProgram.Compile();
            viewModel.CompilationResult = result;

            if (result.Status == UserProgram.Result.StatusType.Failed)
            {
                Directory.Delete(parentFolderPath, true);
                viewModel.CompilationResult = result;
                return View(viewModel);
            }

            int passed = 0;
            foreach (Test t in problem.Tests)
            {
                result = userProgram.Execute(t.GivenInput);
                ResultViewModel.TestResult testResult = new ResultViewModel.TestResult() { Test = t, ExecutionResult = result };

                if (result.Status == UserProgram.Result.StatusType.Successful)
                {
                    result = userProgram.Evaluate(t.ExpectedOutput.Trim());

                    if (result.Status == UserProgram.Result.StatusType.Successful)
                        passed++;

                    testResult.EvaluationResult = result;
                    viewModel.TestResults.Add(testResult);
                }else
                {
                    viewModel.TestResults.Add(testResult);
                }
            }

            viewModel.PassedTests = passed;

            try
            {
                Directory.Delete(parentFolderPath, true);
            }
            catch (UnauthorizedAccessException)
            {
                Thread.Sleep(0);
                Directory.Delete(parentFolderPath, true);
            }
            

            return View(viewModel);
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

            Problem problem = await dbContext.Problems.Include(m => m.Examples).FirstOrDefaultAsync(p => p.ProblemID == id);

            if (problem == null)
                return NotFound();

            SolveViewModel solution = new SolveViewModel()
            {
                ProblemID = problem.ProblemID,
                Name = problem.Name,
                Description = problem.Description,
                InputDescription = problem.InputDescription,
                OutputDescription = problem.OutputDescription,
                Examples = problem.Examples
            };

            return View("Solve", solution);
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
        public async Task<IActionResult> Solve(SolveViewModel solutionViewModel)
        {
            if (ModelState.IsValid)
            {
                if (solutionViewModel.SourceFile?.Length > 0)
                {
                    string randomName = Guid.NewGuid().ToString().Substring(0, 16);
                    string uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");                 
                    Directory.CreateDirectory(uploads);

                    string folderPath = Path.Combine(uploads, randomName);
                    Directory.CreateDirectory(folderPath);

                    string filePath = Path.Combine(folderPath, randomName + Path.GetExtension(solutionViewModel.SourceFile.FileName));      

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await solutionViewModel.SourceFile.CopyToAsync(stream);
                    }

                    return RedirectToAction(nameof(Load), new { id = solutionViewModel.ProblemID, filePath });
                }
            }

            return await Solve(solutionViewModel.ProblemID);
        }
    }
}