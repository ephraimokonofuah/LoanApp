using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "GlobalAdmin,Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
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
            ViewBag.AdminUsers = userList.Count(u => u.Roles.Contains("Admin") || u.Roles.Contains("GlobalAdmin"));

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

            // Prevent regular Admin from banning a GlobalAdmin
            if (await _userManager.IsInRoleAsync(user, "GlobalAdmin") && !await _userManager.IsInRoleAsync(currentUser!, "GlobalAdmin"))
            {
                TempData["error"] = "Only a Global Admin can ban another Global Admin.";
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
        public async Task<IActionResult> PromoteToGlobalAdmin(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(currentUser!, "GlobalAdmin"))
            {
                TempData["error"] = "Only a Global Admin can promote users to Global Admin.";
                return RedirectToAction("Details", new { id });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync("GlobalAdmin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("GlobalAdmin"));
            }

            if (!await _userManager.IsInRoleAsync(user, "GlobalAdmin"))
            {
                // Remove existing roles
                if (await _userManager.IsInRoleAsync(user, "User"))
                    await _userManager.RemoveFromRoleAsync(user, "User");
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    await _userManager.RemoveFromRoleAsync(user, "Admin");

                await _userManager.AddToRoleAsync(user, "GlobalAdmin");
                await _userManager.UpdateSecurityStampAsync(user);
                TempData["success"] = $"{user.FullName ?? user.Email} has been promoted to Global Admin.";
            }
            else
            {
                TempData["info"] = "User is already a Global Admin.";
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

            // Prevent regular Admin from demoting a GlobalAdmin
            if (await _userManager.IsInRoleAsync(user, "GlobalAdmin") && !await _userManager.IsInRoleAsync(currentUser!, "GlobalAdmin"))
            {
                TempData["error"] = "Only a Global Admin can demote another Global Admin.";
                return RedirectToAction("Details", new { id });
            }

            // Remove GlobalAdmin or Admin role
            if (await _userManager.IsInRoleAsync(user, "GlobalAdmin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "GlobalAdmin");
            }
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }

            // Add User role back
            if (!await _userManager.IsInRoleAsync(user, "User"))
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            await _userManager.UpdateSecurityStampAsync(user);
            TempData["success"] = $"{user.FullName ?? user.Email} has been demoted to User.";

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GlobalAdmin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser?.Id)
            {
                TempData["error"] = "You cannot delete your own account.";
                return RedirectToAction("Details", new { id });
            }

            if (await _userManager.IsInRoleAsync(user, "GlobalAdmin"))
            {
                TempData["error"] = "You cannot delete another Global Admin account. Demote them first.";
                return RedirectToAction("Details", new { id });
            }

            var userId = user.Id;
            var userName = user.FullName ?? user.Email;

            // 1. Unassign from support tickets where this user is assigned admin
            var assignedTickets = _unitOfWork.SupportTicket.GetAll(t => t.AssignedToId == userId).ToList();
            foreach (var ticket in assignedTickets)
            {
                ticket.AssignedToId = null;
                _unitOfWork.SupportTicket.Update(ticket);
            }
            _unitOfWork.Save();

            // 2. Delete ticket messages sent by user on OTHER users' tickets
            var sentMessages = _unitOfWork.TicketMessage.GetAll(
                m => m.SenderId == userId, includeProperties: "SupportTicket"
            ).Where(m => m.SupportTicket.UserId != userId).ToList();
            _unitOfWork.TicketMessage.RemoveRange(sentMessages);

            // 3. Delete user's own support tickets (cascade deletes their messages)
            var userTickets = _unitOfWork.SupportTicket.GetAll(t => t.UserId == userId).ToList();
            _unitOfWork.SupportTicket.RemoveRange(userTickets);
            _unitOfWork.Save();

            // 4. Delete repayments
            var repayments = _unitOfWork.Repayment.GetAll(r => r.UserId == userId).ToList();
            _unitOfWork.Repayment.RemoveRange(repayments);

            // 5. Delete loans
            var loans = _unitOfWork.Loan.GetAll(l => l.UserId == userId).ToList();
            _unitOfWork.Loan.RemoveRange(loans);

            // 6. Delete disbursements
            var disbursements = _unitOfWork.LoanDisbursement.GetAll(d => d.UserId == userId).ToList();
            _unitOfWork.LoanDisbursement.RemoveRange(disbursements);
            _unitOfWork.Save();

            // 7. Delete documents + physical files
            var applications = _unitOfWork.LoanApplication.GetAll(a => a.UserId == userId).ToList();
            foreach (var app in applications)
            {
                var documents = _unitOfWork.Document.GetDocumentsByApplicationId(app.Id).ToList();
                foreach (var doc in documents)
                {
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, doc.FilePath.TrimStart('/', '\\'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _unitOfWork.Document.RemoveRange(documents);
            }

            // 8. Delete loan applications
            _unitOfWork.LoanApplication.RemoveRange(applications);

            // 9. Delete eligibility checks
            var eligibilityChecks = _unitOfWork.EligibilityCheck.GetAll(e => e.UserId == userId).ToList();
            _unitOfWork.EligibilityCheck.RemoveRange(eligibilityChecks);
            _unitOfWork.Save();

            // 10. Remove all roles and delete the user
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
            }
            await _userManager.DeleteAsync(user);

            TempData["success"] = $"User \"{userName}\" and all related records have been permanently deleted.";
            return RedirectToAction("Index");
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
