using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "User")]
    public class RepaymentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepaymentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var repayments = _unitOfWork.Repayment
                .GetAll(r => r.UserId == userId, includeProperties: "Loan")
                .OrderBy(r => r.DueDate)
                .ToList();

            // Update statuses based on current date
            UpdateRepaymentStatuses(repayments);

            return View(repayments);
        }

        public IActionResult Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var repayment = _unitOfWork.Repayment.Get(
                r => r.Id == id && r.UserId == userId,
                includeProperties: "Loan", tracked: true);

            if (repayment == null) return NotFound();

            if (!repayment.IsReadByUser)
            {
                repayment.IsReadByUser = true;
                _unitOfWork.Save();
            }

            return View(repayment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestPayment(int id, PaymentMethodType paymentMethod)
        {
            var userId = _userManager.GetUserId(User);
            var repayment = _unitOfWork.Repayment.Get(
                r => r.Id == id && r.UserId == userId, tracked: true);

            if (repayment == null) return NotFound();

            if (repayment.Status != RepaymentStatus.Due && repayment.Status != RepaymentStatus.Missed && repayment.Status != RepaymentStatus.Upcoming)
            {
                TempData["error"] = "Payment request is not available for this installment.";
                return RedirectToAction("Details", new { id });
            }

            // For early payments, ensure all prior installments are paid or already in progress
            if (repayment.Status == RepaymentStatus.Upcoming)
            {
                var priorUnpaid = _unitOfWork.Repayment.GetAll(
                    r => r.LoanId == repayment.LoanId
                         && r.InstallmentNumber < repayment.InstallmentNumber
                         && r.Status != RepaymentStatus.Paid
                         && r.Status != RepaymentStatus.PaymentRequested
                         && r.Status != RepaymentStatus.DetailsSent);

                if (priorUnpaid.Any())
                {
                    TempData["error"] = "Please complete or request payment for earlier installments first.";
                    return RedirectToAction("Details", new { id });
                }
            }

            if (paymentMethod == PaymentMethodType.None)
            {
                TempData["error"] = "Please select a payment method.";
                return RedirectToAction("Details", new { id });
            }

            repayment.PaymentMethodRequested = paymentMethod;
            repayment.PaymentRequestedAt = DateTime.UtcNow;
            repayment.Status = RepaymentStatus.PaymentRequested;
            repayment.IsReadByAdmin = false;
            repayment.IsReadByUser = true;

            _unitOfWork.Repayment.Update(repayment);
            _unitOfWork.Save();

            TempData["success"] = $"Payment details for {paymentMethod} have been requested. You will be notified once the admin provides the details.";
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmPayment(int id, string transactionReference)
        {
            var userId = _userManager.GetUserId(User);
            var repayment = _unitOfWork.Repayment.Get(
                r => r.Id == id && r.UserId == userId, tracked: true);

            if (repayment == null) return NotFound();

            if (repayment.Status != RepaymentStatus.DetailsSent)
            {
                TempData["error"] = "You can only confirm payment after receiving payment details.";
                return RedirectToAction("Details", new { id });
            }

            repayment.TransactionReference = transactionReference;
            repayment.IsReadByAdmin = false;
            repayment.IsReadByUser = true;

            _unitOfWork.Repayment.Update(repayment);
            _unitOfWork.Save();

            TempData["success"] = "Transaction reference submitted. Admin will verify and confirm your payment.";
            return RedirectToAction("Details", new { id });
        }

        private void UpdateRepaymentStatuses(List<Repayment> repayments)
        {
            var changed = false;
            foreach (var r in repayments)
            {
                if (r.Status == RepaymentStatus.Upcoming && r.DueDate.Date <= DateTime.UtcNow.Date)
                {
                    r.Status = RepaymentStatus.Due;
                    _unitOfWork.Repayment.Update(r);
                    changed = true;
                }
                else if (r.Status == RepaymentStatus.Due && r.DueDate.Date < DateTime.UtcNow.Date.AddDays(-7))
                {
                    r.Status = RepaymentStatus.Missed;
                    r.IsReadByUser = false;
                    r.IsReadByAdmin = false;
                    _unitOfWork.Repayment.Update(r);
                    changed = true;
                }
            }
            if (changed) _unitOfWork.Save();
        }
    }
}
