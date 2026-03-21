using LoanApp.Models;
using LoanApp.Models.ViewModels;
using LoanApp.Repository.IRepository;
using LoanApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "User")]
    public class EligibilityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public EligibilityController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var checks = _unitOfWork.EligibilityCheck.GetAll(
                e => e.UserId == user!.Id, includeProperties: "LoanType").ToList();
            return View(checks);
        }

        public async Task<IActionResult> Check()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.LoanTypes = _unitOfWork.LoanType.GetAll(lt => lt.IsActive).ToList();
            var model = new EligibilityCheckViewModel
            {
                FullName = user?.FullName ?? string.Empty
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Check(EligibilityCheckViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LoanTypes = _unitOfWork.LoanType.GetAll(lt => lt.IsActive).ToList();
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var loanType = _unitOfWork.LoanType.Get(lt => lt.Id == model.LoanTypeId);
            if (loanType == null)
            {
                ModelState.AddModelError("LoanTypeId", "Invalid loan type selected.");
                ViewBag.LoanTypes = _unitOfWork.LoanType.GetAll(lt => lt.IsActive).ToList();
                return View(model);
            }

            // Run eligibility rules
            var (isEligible, reason) = EvaluateEligibility(model, loanType.InterestRate);

            // Calculate EMI
            var (monthly, total, interest) = LoanCalculator.Calculate(
                model.DesiredLoanAmount, loanType.InterestRate, model.DurationMonths);

            var eligibilityCheck = new EligibilityCheck
            {
                UserId = user.Id,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                EmploymentStatus = model.EmploymentStatus,
                MonthlyIncome = model.MonthlyIncome,
                MonthlyExpenses = model.MonthlyExpenses,
                CreditScoreRange = model.CreditScoreRange,
                LoanTypeId = model.LoanTypeId,
                DesiredLoanAmount = model.DesiredLoanAmount,
                DurationMonths = model.DurationMonths,
                IsEligible = isEligible,
                EligibilityReason = reason,
                InterestRate = loanType.InterestRate,
                MonthlyRepayment = monthly,
                TotalRepayment = total,
                TotalInterest = interest,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.EligibilityCheck.Add(eligibilityCheck);
            _unitOfWork.Save();

            return RedirectToAction("Result", new { id = eligibilityCheck.Id });
        }

        public async Task<IActionResult> Result(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var check = _unitOfWork.EligibilityCheck.Get(
                e => e.Id == id && e.UserId == user!.Id, includeProperties: "LoanType", tracked: true);
            if (check == null) return NotFound();

            if (!check.IsReadByUser)
            {
                check.IsReadByUser = true;
                _unitOfWork.Save();
            }

            return View(check);
        }

        private (bool IsEligible, string Reason) EvaluateEligibility(EligibilityCheckViewModel model, decimal interestRate)
        {
            var reasons = new List<string>();

            // Age check: must be 18 or older
            var age = DateTime.UtcNow.Year - model.DateOfBirth.Year;
            if (model.DateOfBirth > DateTime.UtcNow.AddYears(-age)) age--;
            if (age < 18)
                reasons.Add("Applicant must be at least 18 years old.");

            // Employment check
            if (model.EmploymentStatus == EmploymentStatus.Unemployed)
                reasons.Add("Applicant must be employed or self-employed.");

            // Credit score check
            if (model.CreditScoreRange == CreditScoreRange.Poor)
                reasons.Add("Credit score is too low for loan approval.");

            if (reasons.Count == 0)
                return (true, "You meet all eligibility criteria.");

            return (false, string.Join(" ", reasons));
        }
    }
}
