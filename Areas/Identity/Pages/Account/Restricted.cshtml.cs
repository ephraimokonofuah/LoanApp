using LoanApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoanApp.Areas.Identity.Pages.Account
{
    public class RestrictedModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RestrictedModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string? BanReason { get; set; }
        public DateTime? BannedAt { get; set; }
        public string? UserName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsBanned)
            {
                return Redirect("/");
            }

            UserName = user.FullName?.Split(' ').FirstOrDefault() ?? "User";
            BanReason = user.BanReason;
            BannedAt = user.BannedAt;
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }
    }
}
