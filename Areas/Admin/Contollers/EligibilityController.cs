using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "GlobalAdmin,Admin")]
    public class EligibilityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public EligibilityController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var checks = _unitOfWork.EligibilityCheck.GetAll(includeProperties: "LoanType,User").ToList();

            ViewBag.TotalChecks = checks.Count;
            ViewBag.Pending = checks.Count(c => c.Status == "Pending");
            ViewBag.Approved = checks.Count(c => c.Status == "Approved");
            ViewBag.Rejected = checks.Count(c => c.Status == "Rejected");
            ViewBag.Eligible = checks.Count(c => c.IsEligible);

            return View(checks);
        }

        public IActionResult Details(int id)
        {
            var check = _unitOfWork.EligibilityCheck.Get(
                e => e.Id == id, includeProperties: "LoanType,User", tracked: true);
            if (check == null) return NotFound();

            if (!check.IsReadByAdmin)
            {
                check.IsReadByAdmin = true;
                _unitOfWork.Save();
            }

            // Check if there's a linked loan application
            var linkedApplication = _unitOfWork.LoanApplication.Get(
                la => la.UserId == check.UserId
                    && la.LoanTypeId == check.LoanTypeId
                    && la.LoanAmount == check.DesiredLoanAmount);
            ViewBag.LinkedApplication = linkedApplication;

            return View(check);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string? adminNotes)
        {
            var check = _unitOfWork.EligibilityCheck.Get(e => e.Id == id, tracked: true);
            if (check == null) return NotFound();

            var admin = await _userManager.GetUserAsync(User);
            check.Status = "Approved";
            check.AdminNotes = adminNotes;
            check.ReviewedBy = admin!.Id;
            check.ReviewedAt = DateTime.UtcNow;
            check.IsReadByUser = false;

            _unitOfWork.EligibilityCheck.Update(check);
            _unitOfWork.Save();

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string? adminNotes)
        {
            var check = _unitOfWork.EligibilityCheck.Get(e => e.Id == id, tracked: true);
            if (check == null) return NotFound();

            var admin = await _userManager.GetUserAsync(User);
            check.Status = "Rejected";
            check.AdminNotes = adminNotes;
            check.ReviewedBy = admin!.Id;
            check.ReviewedAt = DateTime.UtcNow;
            check.IsReadByUser = false;

            _unitOfWork.EligibilityCheck.Update(check);
            _unitOfWork.Save();

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GlobalAdmin")]
        public IActionResult Delete(int id)
        {
            var check = _unitOfWork.EligibilityCheck.Get(e => e.Id == id);
            if (check == null) return NotFound();

            _unitOfWork.EligibilityCheck.Remove(check);
            _unitOfWork.Save();

            TempData["success"] = $"Eligibility Check #{id} has been permanently deleted.";
            return RedirectToAction("Index");
        }
    }
}
