using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUTSWeb.Areas.Identity.Data;
using PUTSWeb.Models;
using System;
using System.Threading.Tasks;

namespace PUTSWeb.Controllers
{
    [Authorize (Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly ProblemDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public BlogController(ProblemDbContext problemDbContext, UserManager<ApplicationUser> userMngr)
        {
            dbContext = problemDbContext;
            userManager = userMngr;
        }

        public IActionResult Create()
        {
            Blog p = new Blog();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser currentUser = await userManager.GetUserAsync(User);

                    blog.AddedDate = DateTime.Now;
                    blog.Author = currentUser.FirstName + ' ' + currentUser.LastName;
                    dbContext.Blogs.Add(blog);
                    await dbContext.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to create a blog");
            }

            return View(blog);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Blog blog = await dbContext.Blogs.FirstOrDefaultAsync(b => b.BlogID == id);

            if (blog == null)
                return NotFound();

            return View(blog);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlog(int? id)
        {
            if (id == null)
                return NotFound();

            Blog blogToEdit = await dbContext.Blogs.FirstOrDefaultAsync(b => b.BlogID == id);

            if (await TryUpdateModelAsync(blogToEdit))
            {
                try
                {
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. ");
                    return View(blogToEdit);
                }
            }

            return View(blogToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await dbContext.Blogs.FirstOrDefaultAsync(b => b.BlogID == id);
            if (blog == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                dbContext.Blogs.Remove(blog);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
