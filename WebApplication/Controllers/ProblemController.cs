﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ProblemController : Controller
    {
        ProblemDbContext dbContext;
        readonly IHostingEnvironment hostingEnvironment; 

        const int pageSize = 20;

        public ProblemController(ProblemDbContext problemDbContext, IHostingEnvironment hostingEnv)
        {
            dbContext = problemDbContext;
            hostingEnvironment = hostingEnv;
        }

        public async Task<IActionResult> List(string sortString, string currentSearchString, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentSearchString;
            }

            ViewData["CurrentSearchString"] = searchString;
            var problemList = from m in dbContext.Problems select m;
            problemList = problemList.OrderByDescending(m => m.ProblemID); // Order by ID descending (default)

            if (!string.IsNullOrEmpty(searchString))
            {
                // Search name and ID
                problemList = problemList.Where(m => (
                m.ProblemID.ToString().IndexOf(searchString, StringComparison.CurrentCultureIgnoreCase) >= 0 
                || m.Name.IndexOf(searchString, StringComparison.CurrentCultureIgnoreCase) >= 0));
            }

            if (!string.IsNullOrEmpty(sortString))
            {
                HandleSortString(sortString);

                switch (sortString)
                {
                    case ("IDAscending"):
                        problemList = problemList.OrderBy(m => m.ProblemID);
                        break;
                    case ("IDDescending"):
                        problemList = problemList.OrderByDescending(m => m.ProblemID);
                        break;
                    case ("NameAscending"):
                        problemList = problemList.OrderBy(m => m.Name);
                        break;
                    case ("NameDescending"):
                        problemList = problemList.OrderByDescending(m => m.Name);
                        break;
                    case ("SolvedAscending"):
                        problemList = problemList.OrderBy(m => m.TimesSolved);
                        break;
                    case ("SolvedDescending"):
                        problemList = problemList.OrderByDescending(m => m.TimesSolved);
                        break;
                    case ("DateAscending"):
                        problemList = problemList.OrderBy(m => m.AddedDate);
                        break;
                    case ("DateDescending"):
                        problemList = problemList.OrderByDescending(m => m.AddedDate);
                        break;
                    default:
                        break;
                }
            }else
            {
                RestoreSortOptions();
            }

            return View(await PaginatedList<Problem>.CreateAsync(problemList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize (Roles = "Admin")]
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
                ProblemID = problem.ProblemID,
                Name = problem.Name,
                ShowFailedTestCases = problem.ShowFailedTestCases
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
                    result = userProgram.EvaluateAndGetResultIfFailed(t.ExpectedOutput.Trim());

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
            catch (UnauthorizedAccessException) // Sometimes the process isn't killed fast enough
            {
                Thread.Sleep(100);
                Directory.Delete(parentFolderPath, true);
            }
            
            if(passed == problem.Tests.Count)
            {
                problem.TimesSolved++;

                if (await TryUpdateModelAsync(problem))
                {
                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        return View(viewModel);
                    }
                }
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Problem problem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    problem.AddedDate = DateTime.Now;
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        void RestoreSortOptions()
        {
            ViewData["IDSort"] = "IDAscending";
            ViewData["NameSort"] = "NameAscending";
            ViewData["SolvedSort"] = "SolvedAscending";
            ViewData["DateSort"] = "DateAscending";
        }

        void HandleSortString(string sortString)
        {
            if (sortString == "IDAscending")
            {
                RestoreSortOptions();
                ViewData["IDSort"] = "IDDescending";
            }
            else if (sortString == "IDDescending")
            {
                RestoreSortOptions();
                ViewData["IDSort"] = "IDAscending";
            }

            if (sortString == "NameAscending")
            {
                RestoreSortOptions();
                ViewData["NameSort"] = "NameDescending";
            }
            else if (sortString == "NameDescending")
            {
                RestoreSortOptions();
                ViewData["NameSort"] = "NameAscending";
            }

            if (sortString == "SolvedAscending")
            {
                RestoreSortOptions();
                ViewData["SolvedSort"] = "SolvedDescending";
            }
            else if (sortString == "SolvedDescending")
            {
                RestoreSortOptions();
                ViewData["SolvedSort"] = "SolvedAscending";
            }

            if (sortString == "DateAscending")
            {
                RestoreSortOptions();
                ViewData["DateSort"] = "DateDescending";
            }
            else if (sortString == "DateDescending")
            {
                RestoreSortOptions();
                ViewData["DateSort"] = "DateAscending";
            }
        }
    }
}