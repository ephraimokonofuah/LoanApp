using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DocumentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DocumentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult GetDocuments(int loanApplicationId)
        {
            var documents = _unitOfWork.Document.GetDocumentsByApplicationId(loanApplicationId).ToList();
            return Json(new { data = documents });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveDocument(int documentId, string? reviewNotes)
        {
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document == null) return NotFound();

            var admin = await _userManager.GetUserAsync(User);
            document.Status = "Approved";
            document.ReviewedAt = DateTime.UtcNow;
            document.ReviewedBy = admin?.Email;
            document.ReviewNotes = reviewNotes;
            _unitOfWork.Document.Update(document);
            _unitOfWork.Save();

            return RedirectToAction("Details", "LoanApplication", new { area = "Admin", id = document.LoanApplicationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectDocument(int documentId, string? reviewNotes)
        {
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document == null) return NotFound();

            var admin = await _userManager.GetUserAsync(User);
            document.Status = "Rejected";
            document.ReviewedAt = DateTime.UtcNow;
            document.ReviewedBy = admin?.Email;
            document.ReviewNotes = reviewNotes;
            _unitOfWork.Document.Update(document);
            _unitOfWork.Save();

            return RedirectToAction("Details", "LoanApplication", new { area = "Admin", id = document.LoanApplicationId });
        }

        [HttpGet]
        public IActionResult ViewDocument(int documentId)
        {
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document == null) return NotFound();

            var normalizedPath = document.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, normalizedPath);

            if (!System.IO.File.Exists(filePath)) return NotFound();

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, document.MimeType ?? "application/octet-stream");
        }

        [HttpGet]
        public IActionResult DownloadDocument(int documentId)
        {
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document == null) return NotFound();

            var normalizedPath = document.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, normalizedPath);

            if (!System.IO.File.Exists(filePath)) return NotFound();

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, document.MimeType ?? "application/octet-stream", document.FileName);
        }
    }
}
