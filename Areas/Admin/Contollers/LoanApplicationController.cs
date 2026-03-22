using LoanApp.Data;
using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LoanApplicationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        public LoanApplicationController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<LoanApplication> loanApplicationList = _unitOfWork.LoanApplication.GetAll().ToList();

            ViewBag.TotalApplications = loanApplicationList.Count;
            ViewBag.Pending = loanApplicationList.Count(a => a.Status == "Pending");
            ViewBag.Approved = loanApplicationList.Count(a => a.Status == "Approved");
            ViewBag.Rejected = loanApplicationList.Count(a => a.Status == "Rejected");
            ViewBag.TotalUsers = _userManager.Users.Count();

            return View(loanApplicationList);
        }

        public IActionResult Details(int id)
        {
            LoanApplication loanApplication = _unitOfWork.LoanApplication.Get(u => u.Id == id, tracked: true);
            if (loanApplication == null)
            {
                return NotFound();
            }

            if (!loanApplication.IsReadByAdmin)
            {
                loanApplication.IsReadByAdmin = true;
                _unitOfWork.Save();
            }

            var documents = _unitOfWork.Document.GetDocumentsByApplicationId(id).ToList();
            ViewBag.Documents = documents;
            return View(loanApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestDocuments(int id, string? documentRequestNote)
        {
            var loanApplication = _unitOfWork.LoanApplication.Get(u => u.Id == id);
            if (loanApplication == null) return NotFound();

            loanApplication.DocumentsRequested = true;
            loanApplication.DocumentRequestNote = documentRequestNote;
            loanApplication.Status = "Documents Requested";
            loanApplication.IsReadByUser = false;
            _unitOfWork.LoanApplication.Update(loanApplication);
            _unitOfWork.Save();

            var user = await _userManager.FindByIdAsync(loanApplication.UserId);
            if (user != null)
            {
                await _emailSender.SendEmailAsync(user.Email!, "Documents Requested for Loan Application",
                    $"Please upload the required documents for your loan application (ID: {loanApplication.Id}). Note: {documentRequestNote ?? "Please upload all supporting documents."}");
            }
            return RedirectToAction("Details", new { id });
        }

        #region API CALLS

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var loanApplication = _unitOfWork.LoanApplication.Get(u => u.Id == id);
            if (loanApplication == null)
                return NotFound();
            loanApplication.Status = "Approved";
            loanApplication.IsReadByUser = false;
            _unitOfWork.LoanApplication.Update(loanApplication);

            // Auto-create disbursement record
            var existingDisbursement = _unitOfWork.LoanDisbursement.Get(d => d.LoanApplicationId == id);
            if (existingDisbursement == null)
            {
                string paymentRef;
                do
                {
                    paymentRef = Random.Shared.Next(100000, 999999).ToString();
                } while (_unitOfWork.LoanDisbursement.Get(d => d.PaymentReference == paymentRef) != null);

                var disbursement = new LoanDisbursement
                {
                    LoanApplicationId = loanApplication.Id,
                    UserId = loanApplication.UserId,
                    ApprovedAmount = loanApplication.LoanAmount,
                    Status = Models.DisbursementStatus.PendingSetup,
                    PaymentReference = paymentRef,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _unitOfWork.LoanDisbursement.Add(disbursement);
            }

            _unitOfWork.Save();
            var user = await _userManager.FindByIdAsync(loanApplication.UserId);
            if (user != null)
            {
                await _emailSender.SendEmailAsync(user.Email!, "Loan Application Approved", $"Your loan application (ID: {loanApplication.Id}) has been approved. Please log in to set up your disbursement details.");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var loanApplication = _unitOfWork.LoanApplication.Get(u => u.Id == id);
            if (loanApplication == null)
                return NotFound();
            loanApplication.Status = "Rejected";
            loanApplication.IsReadByUser = false;
            _unitOfWork.LoanApplication.Update(loanApplication);
            _unitOfWork.Save();
            var user = await _userManager.FindByIdAsync(loanApplication.UserId);
            if (user != null)
            {
                await _emailSender.SendEmailAsync(user.Email!, "Loan Application Rejected", $"Your loan application (ID: {loanApplication.Id}) has been rejected.");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var objLoanApplicationList = _unitOfWork.LoanApplication.GetAll(includeProperties: "User")
                .Select(a => new
                {
                    a.Id,
                    applicantName = a.User != null ? a.User.FullName : "N/A",
                    a.LoanAmount,
                    a.LoanPurpose,
                    a.DurationMonths,
                    a.InterestRate,
                    a.Status,
                    a.CreatedAt
                }).ToList();
            return Json(new { data = objLoanApplicationList });

        } 
        
        #endregion

    }
}