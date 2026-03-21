using LoanApp.Models;
using LoanApp.Models.ViewModels;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "User")]
    public class DisbursementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public DisbursementController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var disbursements = _unitOfWork.LoanDisbursement
                .GetAll(d => d.UserId == user.Id, includeProperties: "LoanApplication,Bank")
                .ToList();

            return View(disbursements);
        }

        public async Task<IActionResult> Setup(int loanApplicationId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var disbursement = _unitOfWork.LoanDisbursement
                .Get(d => d.LoanApplicationId == loanApplicationId && d.UserId == user.Id);
            if (disbursement == null) return NotFound();

            if (disbursement.Status != DisbursementStatus.PendingSetup
                && disbursement.Status != DisbursementStatus.BankSelected)
            {
                return RedirectToAction("Details", new { id = disbursement.Id });
            }

            var banks = _unitOfWork.Bank.GetAll(b => b.IsActive).ToList();

            var vm = new DisbursementSetupViewModel
            {
                DisbursementId = disbursement.Id,
                LoanApplicationId = disbursement.LoanApplicationId,
                BankId = disbursement.BankId ?? 0,
                AccountName = disbursement.AccountName ?? string.Empty,
                AccountNumber = disbursement.AccountNumber ?? string.Empty,
                SortCode = disbursement.SortCode,
                ApprovedAmount = disbursement.ApprovedAmount,
                Banks = banks
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Setup(DisbursementSetupViewModel vm)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
            {
                vm.Banks = _unitOfWork.Bank.GetAll(b => b.IsActive).ToList();
                return View(vm);
            }

            var disbursement = _unitOfWork.LoanDisbursement
                .Get(d => d.Id == vm.DisbursementId && d.UserId == user.Id, tracked: true);
            if (disbursement == null) return NotFound();

            disbursement.BankId = vm.BankId;
            disbursement.AccountName = vm.AccountName;
            disbursement.AccountNumber = vm.AccountNumber;
            disbursement.SortCode = vm.SortCode;
            disbursement.Status = DisbursementStatus.AccountDetailsSubmitted;
            disbursement.UpdatedAt = DateTime.UtcNow;
            disbursement.IsReadByAdmin = false;

            _unitOfWork.Save();
            TempData["success"] = "Bank details submitted successfully. Your disbursement is now under review.";
            return RedirectToAction("Details", new { id = disbursement.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var disbursement = _unitOfWork.LoanDisbursement
                .Get(d => d.Id == id && d.UserId == user.Id, includeProperties: "LoanApplication,Bank", tracked: true);
            if (disbursement == null) return NotFound();

            if (!disbursement.IsReadByUser)
            {
                disbursement.IsReadByUser = true;
                _unitOfWork.Save();
            }

            return View(disbursement);
        }
    }
}
