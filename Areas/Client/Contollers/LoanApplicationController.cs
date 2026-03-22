using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace LoanApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "User")]
    public class LoanApplicationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoanApplicationController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var loanApplications = _unitOfWork.LoanApplication.GetAll(l => l.UserId == user.Id).ToList();

            ViewBag.TotalApplications = loanApplications.Count;
            ViewBag.Pending = loanApplications.Count(a => a.Status == "Pending");
            ViewBag.Approved = loanApplications.Count(a => a.Status == "Approved");
            ViewBag.Rejected = loanApplications.Count(a => a.Status == "Rejected");
            var firstName = user.FullName?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? user.Email;
            ViewBag.UserName = firstName;

            // Check if user has an approved eligibility
            var approvedEligibility = _unitOfWork.EligibilityCheck.Get(
                e => e.UserId == user.Id && e.IsEligible && e.Status == "Approved");
            ViewBag.HasApprovedEligibility = approvedEligibility != null;
            ViewBag.ApprovedEligibilityId = approvedEligibility?.Id;

            // Get disbursement info for approved loans
            var disbursements = _unitOfWork.LoanDisbursement
                .GetAll(d => d.UserId == user.Id)
                .ToDictionary(d => d.LoanApplicationId, d => d);
            ViewBag.Disbursements = disbursements;

            return View(loanApplications);
        }

        public async Task<IActionResult> Create(int? eligibilityId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (eligibilityId.HasValue)
            {
                var check = _unitOfWork.EligibilityCheck.Get(
                    e => e.Id == eligibilityId.Value && e.UserId == user.Id && e.IsEligible && e.Status == "Approved",
                    includeProperties: "LoanType");
                if (check != null)
                {
                    ViewBag.LoanTypes = _unitOfWork.LoanType.GetAll(lt => lt.IsActive).ToList();
                    var model = new LoanApplication
                    {
                        LoanTypeId = check.LoanTypeId,
                        LoanAmount = check.DesiredLoanAmount,
                        DurationMonths = check.DurationMonths,
                        InterestRate = check.InterestRate,
                        LoanPurpose = check.LoanType?.Name ?? ""
                    };
                    ViewBag.EligibilityId = eligibilityId.Value;
                    return View(model);
                }
            }

            // No valid approved eligibility — redirect to eligibility check
            TempData["Info"] = "You need an approved eligibility check before applying for a loan.";
            return RedirectToAction("Check", "Eligibility", new { area = "Client" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoanApplication model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            model.UserId = user.Id;
            model.Status = "Pending";
            model.CreatedAt = System.DateTime.UtcNow;

            if (model.LoanTypeId.HasValue)
            {
                var loanType = _unitOfWork.LoanType.Get(lt => lt.Id == model.LoanTypeId.Value);
                if (loanType != null)
                {
                    model.InterestRate = loanType.InterestRate;
                    model.LoanPurpose = string.IsNullOrWhiteSpace(model.LoanPurpose) ? loanType.Name : model.LoanPurpose;
                }
            }

            _unitOfWork.LoanApplication.Add(model);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetInterestRate(int loanTypeId)
        {
            var loanType = _unitOfWork.LoanType.Get(lt => lt.Id == loanTypeId);
            if (loanType == null) return NotFound();
            return Json(new { interestRate = loanType.InterestRate, name = loanType.Name });
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var loanApplication = _unitOfWork.LoanApplication.Get(l => l.Id == id, tracked: true);
            if (loanApplication == null || loanApplication.UserId != user.Id) return NotFound();

            if (!loanApplication.IsReadByUser)
            {
                loanApplication.IsReadByUser = true;
                _unitOfWork.Save();
            }

            var documents = _unitOfWork.Document.GetDocumentsByApplicationId(id).ToList();
            ViewBag.Documents = documents;
            return View(loanApplication);
        }
    }
}
