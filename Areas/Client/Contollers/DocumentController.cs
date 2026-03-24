using LoanApp.Models;
using LoanApp.Repository.IRepository;
using LoanApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace LoanApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "User")]
    public class DocumentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<DocumentController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public DocumentController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, ILogger<DocumentController> logger, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            _config = config;
        }

        private async Task<bool> UserOwnsApplication(int loanApplicationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var app = _unitOfWork.LoanApplication.Get(l => l.Id == loanApplicationId);
            return app != null && user != null && app.UserId == user.Id;
        }

        private async Task<bool> UserOwnsDocument(int documentId)
        {
            var doc = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (doc == null) return false;
            return await UserOwnsApplication(doc.LoanApplicationId);
        }

        [HttpGet]
        public async Task<IActionResult> Upload(int loanApplicationId)
        {
            if (!await UserOwnsApplication(loanApplicationId)) return NotFound();
            return View(loanApplicationId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int loanApplicationId, IFormFile file, string documentType)
        {
            if (!await UserOwnsApplication(loanApplicationId))
                return BadRequest(new { success = false, message = "Access denied" });

            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { success = false, message = "No file provided" });
                }

                // Validate file
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                var maxFileSize = 5 * 1024 * 1024; // 5MB

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { success = false, message = "Invalid file type" });
                }

                if (file.Length > maxFileSize)
                {
                    return BadRequest(new { success = false, message = "File size exceeds 5MB limit" });
                }

                // Create documents folder if it doesn't exist
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "documents");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                var relativePath = Path.Combine("uploads", "documents", fileName).Replace("\\", "/");

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Create document record
                var document = new Document
                {
                    LoanApplicationId = loanApplicationId,
                    DocumentType = documentType,
                    FileName = file.FileName,
                    FilePath = relativePath,
                    FileSize = file.Length,
                    MimeType = file.ContentType,
                    Status = "Pending"
                };

                _unitOfWork.Document.Add(document);
                _unitOfWork.Save();

                // Notify all admins of new document upload
                var user = await _userManager.GetUserAsync(User);
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                var globalAdmins = await _userManager.GetUsersInRoleAsync("GlobalAdmin");
                var emailBody = EmailTemplates.DocumentUploaded(user?.FullName ?? "User", loanApplicationId, documentType);
                foreach (var admin in admins.Concat(globalAdmins).Where(a => !string.IsNullOrEmpty(a.Email)).DistinctBy(a => a.Email))
                {
                    await _emailSender.SendEmailAsync(admin.Email!, "New Document Uploaded", emailBody);
                }

                return Json(new { success = true, message = "Document uploaded successfully", documentId = document.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document");
                
                // Get the inner exception details
                var innerException = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError($"Inner exception: {innerException}");
                
                return BadRequest(new { success = false, message = $"Error: {innerException}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments(int loanApplicationId)
        {
            if (!await UserOwnsApplication(loanApplicationId))
                return Json(new { data = new List<Document>() });
            var documents = _unitOfWork.Document.GetDocumentsByApplicationId(loanApplicationId).ToList();
            return Json(new { data = documents });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteDocumentRequest request)
        {
            try
            {
                if (request == null || request.DocumentId <= 0)
                {
                    return BadRequest(new { success = false, message = "Invalid document ID" });
                }

                var document = _unitOfWork.Document.Get(d => d.Id == request.DocumentId);
                if (document == null)
                {
                    _logger.LogWarning($"Document with ID {request.DocumentId} not found");
                    return BadRequest(new { success = false, message = "Document not found" });
                }

                if (!await UserOwnsDocument(request.DocumentId))
                    return BadRequest(new { success = false, message = "Access denied" });

                _logger.LogInformation($"Attempting to delete document ID {request.DocumentId}, FilePath: {document.FilePath}");

                // Delete file from disk - normalize the path separator
                var normalizedPath = document.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString());
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, normalizedPath);
                
                _logger.LogInformation($"Full file path: {filePath}");
                _logger.LogInformation($"WebRootPath: {_hostEnvironment.WebRootPath}");
                _logger.LogInformation($"File exists: {System.IO.File.Exists(filePath)}");
                
                // Attempt to delete file
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        // Wait a moment to ensure file is not locked
                        System.IO.File.Delete(filePath);
                        _logger.LogInformation($"✓ File deleted successfully from disk: {filePath}");
                    }
                    catch (UnauthorizedAccessException accessEx)
                    {
                        _logger.LogError($"✗ Access denied deleting file: {accessEx.Message}");
                        return BadRequest(new { success = false, message = $"Access denied: Cannot delete file. {accessEx.Message}" });
                    }
                    catch (IOException ioEx)
                    {
                        _logger.LogError($"✗ IO Error deleting file: {ioEx.Message}");
                        return BadRequest(new { success = false, message = $"File error: {ioEx.Message}" });
                    }
                    catch (Exception fileEx)
                    {
                        _logger.LogError($"✗ Error deleting file: {fileEx.Message}");
                        return BadRequest(new { success = false, message = $"Could not delete file: {fileEx.Message}" });
                    }
                }
                else
                {
                    _logger.LogWarning($"File does not exist at path: {filePath}");
                    _logger.LogWarning($"Checking if directory exists: {Path.GetDirectoryName(filePath)}");
                }

                // Delete from database
                _unitOfWork.Document.Remove(document);
                _unitOfWork.Save();
                _logger.LogInformation($"✓ Document removed from database successfully");

                return Json(new { success = true, message = "Document deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document");
                var errorMsg = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError($"Exception details: {errorMsg}");
                return BadRequest(new { success = false, message = $"Error: {errorMsg}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Download(int documentId)
        {
            if (!await UserOwnsDocument(documentId)) return NotFound();
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document == null)
            {
                return NotFound();
            }

            // Normalize path separators
            var normalizedPath = document.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, normalizedPath);
            
            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogWarning($"Download - File not found: {filePath}");
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, document.MimeType ?? "application/octet-stream", document.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> ViewDocument(int documentId)
        {
            if (!await UserOwnsDocument(documentId)) return NotFound();
            var document = _unitOfWork.Document.Get(d => d.Id == documentId);
            if (document == null)
            {
                return NotFound();
            }

            // Normalize path separators
            var normalizedPath = document.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, normalizedPath);
            
            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogWarning($"View - File not found: {filePath}");
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            
            // Return file inline so browser can display it
            return File(bytes, document.MimeType ?? "application/octet-stream");
        }
    }

    public class DeleteDocumentRequest
    {
        public int DocumentId { get; set; }
    }
}
