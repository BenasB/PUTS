using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PUTSWeb.Areas.Identity.Data;

namespace PUTSWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> MakeAdmin(string username, bool makeAdmin)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }
            
            if (await _userManager.IsInRoleAsync(user, "SuperUser"))
            {
                return NotFound($"Can't change permissions of a SuperUser");
            }

            if (makeAdmin)
                await _userManager.AddToRoleAsync(user, "Admin");
            else
                await _userManager.RemoveFromRoleAsync(user, "Admin");

            return Content("Admin permissions to the user changed.");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeModerator(string username, bool makeModerator)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            if (await _userManager.IsInRoleAsync(user, "SuperUser"))
            {
                return NotFound($"Can't change permissions of a SuperUser");
            }

            if (makeModerator)
                await _userManager.AddToRoleAsync(user, "Moderator");
            else
                await _userManager.RemoveFromRoleAsync(user, "Moderator");

            return Content("Moderator permissions to the user changed.");
        }
    }
}