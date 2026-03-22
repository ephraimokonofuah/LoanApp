using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "GlobalAdmin,Admin")]
    public class SettingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var settings = _unitOfWork.SiteSettings.Get(s => s.Id == 1);
            if (settings == null)
            {
                settings = new SiteSettings();
                _unitOfWork.SiteSettings.Add(settings);
                _unitOfWork.Save();
            }
            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SiteSettings settings)
        {
            if (ModelState.IsValid)
            {
                settings.Id = 1;
                settings.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.SiteSettings.Update(settings);
                _unitOfWork.Save();
                TempData["success"] = "Site settings updated successfully.";
                return RedirectToAction("Index");
            }
            return View(settings);
        }
    }
}
