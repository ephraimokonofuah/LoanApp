using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "GlobalAdmin,Admin")]
    public class LoanTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoanTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var loanTypes = _unitOfWork.LoanType.GetAll().ToList();
            return View(loanTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LoanType loanType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.LoanType.Add(loanType);
                _unitOfWork.Save();
                TempData["success"] = "Loan type created successfully.";
                return RedirectToAction("Index");
            }
            return View(loanType);
        }

        public IActionResult Edit(int id)
        {
            var loanType = _unitOfWork.LoanType.Get(u => u.Id == id);
            if (loanType == null) return NotFound();
            return View(loanType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LoanType loanType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.LoanType.Update(loanType);
                _unitOfWork.Save();
                TempData["success"] = "Loan type updated successfully.";
                return RedirectToAction("Index");
            }
            return View(loanType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(int id)
        {
            var loanType = _unitOfWork.LoanType.Get(u => u.Id == id, tracked: true);
            if (loanType == null) return NotFound();

            loanType.IsActive = !loanType.IsActive;
            _unitOfWork.Save();
            TempData["success"] = loanType.IsActive ? "Loan type activated." : "Loan type deactivated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var loanType = _unitOfWork.LoanType.Get(u => u.Id == id);
            if (loanType == null) return NotFound();

            var hasApplications = _unitOfWork.LoanApplication.GetAll(a => a.LoanTypeId == id).Any();
            if (hasApplications)
            {
                TempData["error"] = "Cannot delete loan type that has existing applications. Deactivate it instead.";
                return RedirectToAction("Index");
            }

            _unitOfWork.LoanType.Remove(loanType);
            _unitOfWork.Save();
            TempData["success"] = "Loan type deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
