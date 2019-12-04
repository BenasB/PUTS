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

            if (makeAdmin)
                await _userManager.AddToRoleAsync(user, "Admin");
            else
                await _userManager.RemoveFromRoleAsync(user, "Admin");

            return Content("Admin permissions to the user changed.");
        }
    }
}