using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoanApp.Models;

namespace LoanApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("/PostLogin")]
        public async Task<IActionResult> PostLogin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Redirect("/Identity/Account/Login");

            // Auto-assign "User" role if user has no roles yet
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, "User");
                // Refresh the sign-in cookie so the new role claim is included
                await _signInManager.RefreshSignInAsync(user);
            }

            if (await _userManager.IsInRoleAsync(user, "GlobalAdmin") || await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "LoanApplication", new { area = "Admin" });
            }

            return RedirectToAction("Index", "LoanApplication", new { area = "Client" });
        }
    }
}
