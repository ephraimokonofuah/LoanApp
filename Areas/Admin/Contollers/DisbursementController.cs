using LoanApp.Models;
using LoanApp.Models.ViewModels;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DisbursementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DisbursementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public IActionResult UpdateStatus(DisbursementUpdateViewModel vm)
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
            }

            _unitOfWork.Save();
            TempData["success"] = $"Disbursement status updated to {vm.NewStatus}.";
            return RedirectToAction("Details", new { id = vm.DisbursementId });
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
