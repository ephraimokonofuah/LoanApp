using LoanApp.Models;
using LoanApp.Models.ViewModels;
using LoanApp.Repository.IRepository;
using LoanApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "GlobalAdmin,Admin")]
    public class DisbursementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public DisbursementController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var disbursements = _unitOfWork.LoanDisbursement
                .GetAll(includeProperties: "LoanApplication,User,Bank")
                .ToList();
            return View(disbursements);
        }

        public IActionResult Details(int id)
        {
            var disbursement = _unitOfWork.LoanDisbursement
                .Get(d => d.Id == id, includeProperties: "LoanApplication,User,Bank", tracked: true);
            if (disbursement == null) return NotFound();

            if (!disbursement.IsReadByAdmin)
            {
                disbursement.IsReadByAdmin = true;
                _unitOfWork.Save();
            }

            var vm = new DisbursementUpdateViewModel
            {
                DisbursementId = disbursement.Id,
                LoanApplicationId = disbursement.LoanApplicationId,
                ApplicantName = disbursement.User?.FullName ?? "N/A",
                BankName = disbursement.Bank?.Name,
                AccountName = disbursement.AccountName,
                AccountNumber = disbursement.AccountNumber,
                SortCode = disbursement.SortCode,
                ApprovedAmount = disbursement.ApprovedAmount,
                CurrentStatus = disbursement.Status,
                NewStatus = disbursement.Status,
                PaidAmount = disbursement.PaidAmount > 0 ? disbursement.PaidAmount : null,
                PaymentReference = disbursement.PaymentReference,
                Notes = disbursement.Notes
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(DisbursementUpdateViewModel vm)
        {
            var disbursement = _unitOfWork.LoanDisbursement
                .Get(d => d.Id == vm.DisbursementId, tracked: true);
            if (disbursement == null) return NotFound();

            // Validate status transitions
            if (!IsValidTransition(disbursement.Status, vm.NewStatus))
            {
                TempData["error"] = $"Invalid status transition from {disbursement.Status} to {vm.NewStatus}.";
                return RedirectToAction("Details", new { id = vm.DisbursementId });
            }

            disbursement.Status = vm.NewStatus;
            disbursement.Notes = vm.Notes;
            disbursement.UpdatedAt = DateTime.UtcNow;
            disbursement.IsReadByUser = false;

            if (vm.NewStatus == DisbursementStatus.Paid)
            {
                disbursement.PaidAmount = vm.PaidAmount ?? disbursement.ApprovedAmount;
                disbursement.PaidAt = DateTime.UtcNow;

                // Auto-generate repayment schedule
                GenerateRepaymentSchedule(disbursement);
            }

            _unitOfWork.Save();

            // Send email notification to user
            var user = await _userManager.FindByIdAsync(disbursement.UserId);
            if (user != null)
            {
                var statusText = vm.NewStatus.ToString();
                var amount = disbursement.PaidAmount > 0 ? disbursement.PaidAmount : disbursement.ApprovedAmount;
                await _emailSender.SendEmailAsync(user.Email,
                    $"Disbursement Status Update - {statusText}",
                    EmailTemplates.DisbursementStatusUpdate(user.FullName, statusText, disbursement.Id, amount));
            }

            TempData["success"] = $"Disbursement status updated to {vm.NewStatus}.";
            return RedirectToAction("Details", new { id = vm.DisbursementId });
        }

        private void GenerateRepaymentSchedule(LoanDisbursement disbursement)
        {
            var application = _unitOfWork.LoanApplication.Get(
                a => a.Id == disbursement.LoanApplicationId);
            if (application == null) return;

            // Check if loan already exists for this application
            var existingLoan = _unitOfWork.Loan.Get(l => l.LoanApplicationId == application.Id);
            if (existingLoan != null) return;

            var paidAmount = disbursement.PaidAmount;
            var interestRate = application.InterestRate;
            var durationMonths = application.DurationMonths;
            var startDate = DateTime.UtcNow;

            // Create Loan record
            var loan = new Loan
            {
                UserId = disbursement.UserId,
                LoanApplicationId = application.Id,
                PrincipalAmount = paidAmount,
                InterestRate = interestRate,
                DurationMonths = durationMonths,
                StartDate = startDate,
                Status = "Active"
            };
            _unitOfWork.Loan.Add(loan);
            _unitOfWork.Save();

            // Calculate repayment schedule using EMI (same formula shown to user)
            var (monthlyPayment, totalRepayment, totalInterest) = LoanCalculator.Calculate(paidAmount, interestRate, durationMonths);
            var monthlyPrincipal = Math.Round(paidAmount / durationMonths, 2);
            var monthlyInterest = Math.Round(monthlyPayment - monthlyPrincipal, 2);

            for (int i = 1; i <= durationMonths; i++)
            {
                var isLast = i == durationMonths;
                var principal = isLast
                    ? paidAmount - (monthlyPrincipal * (durationMonths - 1))
                    : monthlyPrincipal;
                var interest = isLast
                    ? totalInterest - (monthlyInterest * (durationMonths - 1))
                    : monthlyInterest;
                var amount = principal + interest;

                var dueDate = startDate.AddDays(28 * i);
                var status = RepaymentStatus.Upcoming;
                if (dueDate.Date <= DateTime.UtcNow.Date)
                    status = RepaymentStatus.Due;

                var repayment = new Repayment
                {
                    LoanId = loan.Id,
                    UserId = disbursement.UserId,
                    InstallmentNumber = i,
                    Amount = amount,
                    PrincipalPortion = principal,
                    InterestPortion = interest,
                    DueDate = dueDate,
                    Status = status,
                    IsReadByAdmin = true,
                    IsReadByUser = false
                };
                _unitOfWork.Repayment.Add(repayment);
            }

            // Update application status
            application.Status = "Disbursed";
            _unitOfWork.LoanApplication.Update(application);
            _unitOfWork.Save();
        }

        private static bool IsValidTransition(DisbursementStatus current, DisbursementStatus next)
        {
            return (current, next) switch
            {
                (DisbursementStatus.PendingSetup, DisbursementStatus.Failed) => true,
                (DisbursementStatus.BankSelected, DisbursementStatus.Failed) => true,
                (DisbursementStatus.AccountDetailsSubmitted, DisbursementStatus.ReadyForPayment) => true,
                (DisbursementStatus.AccountDetailsSubmitted, DisbursementStatus.Failed) => true,
                (DisbursementStatus.ReadyForPayment, DisbursementStatus.Paid) => true,
                (DisbursementStatus.ReadyForPayment, DisbursementStatus.Failed) => true,
                _ => false
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GlobalAdmin")]
        public IActionResult Delete(int id)
        {
            var disbursement = _unitOfWork.LoanDisbursement.Get(d => d.Id == id);
            if (disbursement == null) return NotFound();

            // Delete loan + repayments that were generated from this disbursement
            var loan = _unitOfWork.Loan.Get(l => l.LoanApplicationId == disbursement.LoanApplicationId);
            if (loan != null)
            {
                var repayments = _unitOfWork.Repayment.GetAll(r => r.LoanId == loan.Id).ToList();
                _unitOfWork.Repayment.RemoveRange(repayments);
                _unitOfWork.Loan.Remove(loan);
            }

            _unitOfWork.LoanDisbursement.Remove(disbursement);
            _unitOfWork.Save();

            TempData["success"] = $"Disbursement #{id} and related records have been permanently deleted.";
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _unitOfWork.LoanDisbursement
                .GetAll(includeProperties: "LoanApplication,User,Bank")
                .Select(d => new
                {
                    d.Id,
                    d.LoanApplicationId,
                    applicantName = d.User != null ? d.User.FullName : "N/A",
                    bankName = d.Bank != null ? d.Bank.Name : "Not Selected",
                    d.ApprovedAmount,
                    d.PaidAmount,
                    status = d.Status.ToString(),
                    d.CreatedAt
                }).ToList();
            return Json(new { data = list });
        }

        #endregion
    }
}
