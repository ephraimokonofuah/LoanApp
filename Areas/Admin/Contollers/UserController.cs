using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var currentUser = await _userManager.GetUserAsync(User);

            var userList = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var appCount = _unitOfWork.LoanApplication.GetAll(a => a.UserId == user.Id).Count();

                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName ?? "N/A",
                    Email = user.Email ?? "N/A",
                    PhoneNumber = user.PhoneNumber,
                    Country = user.Country,
                    IsBanned = user.IsBanned,
                    BanReason = user.BanReason,
                    BannedAt = user.BannedAt,
                    CreatedAt = user.CreatedAt,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList(),
                    ApplicationCount = appCount,
                    IsCurrentUser = user.Id == currentUser?.Id
                });
            }

            ViewBag.TotalUsers = userList.Count;
            ViewBag.ActiveUsers = userList.Count(u => !u.IsBanned);
            ViewBag.BannedUsers = userList.Count(u => u.IsBanned);
            ViewBag.AdminUsers = userList.Count(u => u.Roles.Contains("Admin"));

            return View(userList);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var currentUser = await _userManager.GetUserAsync(User);

            var applications = _unitOfWork.LoanApplication.GetAll(a => a.UserId == user.Id).ToList();
            var eligibilityChecks = _unitOfWork.EligibilityCheck.GetAll(e => e.UserId == user.Id).ToList();
            var disbursements = _unitOfWork.LoanDisbursement.GetAll(d => d.UserId == user.Id, includeProperties: "Bank").ToList();

            var vm = new UserDetailsViewModel
            {
                Id = user.Id,
                FullName = user.FullName ?? "N/A",
                Email = user.Email ?? "N/A",
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Country = user.Country,
                PostCode = user.PostCode,
                IsBanned = user.IsBanned,
                BanReason = user.BanReason,
                BannedAt = user.BannedAt,
                CreatedAt = user.CreatedAt,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList(),
                IsCurrentUser = user.Id == currentUser?.Id,
                Applications = applications,
                EligibilityChecks = eligibilityChecks,
                Disbursements = disbursements
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ban(string id, string? banReason)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser?.Id)
            {
                TempData["error"] = "You cannot ban your own account.";
                return RedirectToAction("Details", new { id });
            }

            user.IsBanned = true;
            user.BanReason = banReason ?? "Violated terms of service.";
            user.BannedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Force sign out the banned user by updating their security stamp
            await _userManager.UpdateSecurityStampAsync(user);

            TempData["success"] = $"{user.FullName ?? user.Email} has been banned.";
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.IsBanned = false;
            user.BanReason = null;
            user.BannedAt = null;
            await _userManager.UpdateAsync(user);

            TempData["success"] = $"{user.FullName ?? user.Email} has been activated.";
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteToAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                // Remove from User role if present
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "User");
                }
                await _userManager.AddToRoleAsync(user, "Admin");
                await _userManager.UpdateSecurityStampAsync(user);
                TempData["success"] = $"{user.FullName ?? user.Email} has been promoted to Admin.";
            }
            else
            {
                TempData["info"] = "User is already an Admin.";
            }

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DemoteFromAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser?.Id)
            {
                TempData["error"] = "You cannot remove your own admin privileges.";
                return RedirectToAction("Details", new { id });
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                // Add User role back
                if (!await _userManager.IsInRoleAsync(user, "User"))
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                await _userManager.UpdateSecurityStampAsync(user);
                TempData["success"] = $"{user.FullName ?? user.Email} has been demoted to User.";
            }

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.ConfirmEmailAsync(user, token);

            TempData["success"] = $"Email confirmed for {user.FullName ?? user.Email}.";
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = GenerateTemporaryPassword();
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                TempData["success"] = $"Password has been reset for {user.FullName ?? user.Email}. Temporary password: {newPassword}";
            }
            else
            {
                TempData["error"] = "Failed to reset password: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("Details", new { id });
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = _userManager.Users.ToList();
            var currentUser = await _userManager.GetUserAsync(User);
            var result = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new
                {
                    id = user.Id,
                    fullName = user.FullName ?? "N/A",
                    email = user.Email ?? "N/A",
                    phone = user.PhoneNumber ?? "N/A",
                    roles = string.Join(", ", roles),
                    isBanned = user.IsBanned,
                    createdAt = user.CreatedAt.ToString("dd MMM yyyy"),
                    isCurrentUser = user.Id == currentUser?.Id
                });
            }

            return Json(new { data = result });
        }

        #endregion

        private static string GenerateTemporaryPassword()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%&*";

            var password = new char[12];
            password[0] = upper[Random.Shared.Next(upper.Length)];
            password[1] = lower[Random.Shared.Next(lower.Length)];
            password[2] = digits[Random.Shared.Next(digits.Length)];
            password[3] = special[Random.Shared.Next(special.Length)];

            var all = upper + lower + digits + special;
            for (int i = 4; i < password.Length; i++)
            {
                password[i] = all[Random.Shared.Next(all.Length)];
            }

            // Shuffle
            for (int i = password.Length - 1; i > 0; i--)
            {
                int j = Random.Shared.Next(i + 1);
                (password[i], password[j]) = (password[j], password[i]);
            }

            return new string(password);
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public bool IsBanned { get; set; }
        public string? BanReason { get; set; }
        public DateTime? BannedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; } = new();
        public int ApplicationCount { get; set; }
        public bool IsCurrentUser { get; set; }
    }

    public class UserDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? PostCode { get; set; }
        public bool IsBanned { get; set; }
        public string? BanReason { get; set; }
        public DateTime? BannedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsCurrentUser { get; set; }
        public List<LoanApplication> Applications { get; set; } = new();
        public List<EligibilityCheck> EligibilityChecks { get; set; } = new();
        public List<LoanDisbursement> Disbursements { get; set; } = new();
    }
}
