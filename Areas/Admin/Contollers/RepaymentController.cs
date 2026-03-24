using LoanApp.Models;
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
    public class RepaymentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepaymentController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var repayments = _unitOfWork.Repayment
                .GetAll(includeProperties: "Loan,Loan.LoanApplication,User")
                .OrderBy(r => r.DueDate)
                .ToList();

            // Update statuses based on current date
            UpdateRepaymentStatuses(repayments);

            return View(repayments);
        }

        public IActionResult Details(int id)
        {
            var repayment = _unitOfWork.Repayment.Get(
                r => r.Id == id,
                includeProperties: "Loan,User", tracked: true);

            if (repayment == null) return NotFound();

            if (!repayment.IsReadByAdmin)
            {
                repayment.IsReadByAdmin = true;
                _unitOfWork.Save();
            }

            // Load available wallets for the requested payment method
            if (repayment.Status == RepaymentStatus.PaymentRequested &&
                (repayment.PaymentMethodRequested == PaymentMethodType.USDT || repayment.PaymentMethodRequested == PaymentMethodType.Bitcoin))
            {
                var wallets = _unitOfWork.CryptoWallet
                    .GetAll(w => w.WalletType == repayment.PaymentMethodRequested && w.IsActive)
                    .ToList();
                ViewBag.CryptoWallets = wallets;
            }

            return View(repayment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendPaymentDetails(int id, string paymentDetails, int? walletId)
        {
            var repayment = _unitOfWork.Repayment.Get(
                r => r.Id == id, tracked: true);

            if (repayment == null) return NotFound();

            if (repayment.Status != RepaymentStatus.PaymentRequested)
            {
                TempData["error"] = "Payment details can only be sent for requested payments.";
                return RedirectToAction("Details", new { id });
            }

            // If a wallet was selected, auto-build the payment details
            if (walletId.HasValue && walletId > 0)
            {
                var wallet = _unitOfWork.CryptoWallet.Get(w => w.Id == walletId.Value && w.IsActive);
                if (wallet != null)
                {
                    var details = $"{wallet.WalletType} Wallet Address:\n{wallet.WalletAddress}";
                    if (!string.IsNullOrEmpty(wallet.Network))
                        details += $"\nNetwork: {wallet.Network}";
                    if (!string.IsNullOrEmpty(wallet.Label))
                        details += $"\nLabel: {wallet.Label}";
                    if (!string.IsNullOrEmpty(paymentDetails))
                        details += $"\n\nAdditional Notes:\n{paymentDetails}";
                    paymentDetails = details;
                }
            }

            if (string.IsNullOrWhiteSpace(paymentDetails))
            {
                TempData["error"] = "Please provide the payment details or select a wallet.";
                return RedirectToAction("Details", new { id });
            }

            repayment.PaymentDetails = paymentDetails;
            repayment.PaymentDetailsSentAt = DateTime.UtcNow;
            repayment.Status = RepaymentStatus.DetailsSent;
            repayment.IsReadByUser = false;
            repayment.IsReadByAdmin = true;

            _unitOfWork.Repayment.Update(repayment);
            _unitOfWork.Save();

            // Send email notification to user
            var user = await _userManager.FindByIdAsync(repayment.UserId);
            if (user != null)
            {
                await _emailSender.SendEmailAsync(user.Email!, "Payment Details Are Ready",
                    EmailTemplates.PaymentDetailsSent(user.FullName ?? "User", repayment.InstallmentNumber,
                        repayment.Amount, repayment.PaymentMethodRequested.ToString()));
            }

            TempData["success"] = "Payment details sent to the user successfully.";
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var repayment = _unitOfWork.Repayment.Get(
                r => r.Id == id, tracked: true);

            if (repayment == null) return NotFound();

            if (repayment.Status != RepaymentStatus.DetailsSent)
            {
                TempData["error"] = "Payment can only be confirmed after details have been sent.";
                return RedirectToAction("Details", new { id });
            }

            repayment.Status = RepaymentStatus.Paid;
            repayment.PaidAt = DateTime.UtcNow;
            repayment.IsReadByUser = false;
            repayment.IsReadByAdmin = true;

            _unitOfWork.Repayment.Update(repayment);
            _unitOfWork.Save();

            // Send email notification to user
            var paidUser = await _userManager.FindByIdAsync(repayment.UserId);
            if (paidUser != null)
            {
                await _emailSender.SendEmailAsync(paidUser.Email!, "Payment Confirmed",
                    EmailTemplates.PaymentConfirmed(paidUser.FullName ?? "User", repayment.InstallmentNumber, repayment.Amount));
            }

            TempData["success"] = "Payment confirmed successfully.";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GlobalAdmin")]
        public IActionResult Delete(int id)
        {
            var repayment = _unitOfWork.Repayment.Get(r => r.Id == id);
            if (repayment == null) return NotFound();

            var loanId = repayment.LoanId;
            _unitOfWork.Repayment.Remove(repayment);
            _unitOfWork.Save();

            TempData["success"] = $"Repayment installment #{repayment.InstallmentNumber} has been permanently deleted.";
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _unitOfWork.Repayment
                .GetAll(includeProperties: "Loan,User")
                .Select(r => new
                {
                    r.Id,
                    r.InstallmentNumber,
                    applicantName = r.User != null ? r.User.FullName : "N/A",
                    r.Amount,
                    dueDate = r.DueDate.ToString("yyyy-MM-dd"),
                    status = r.Status.ToString(),
                    paymentMethod = r.PaymentMethodRequested.ToString(),
                    r.IsReadByAdmin
                }).ToList();
            return Json(new { data = list });
        }

        #endregion
    }
}
