using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "GlobalAdmin,Admin")]
    public class BankController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var banks = _unitOfWork.Bank.GetAll().ToList();
            return View(banks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bank bank)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Bank.Add(bank);
                _unitOfWork.Save();
                TempData["success"] = "Bank created successfully.";
                return RedirectToAction("Index");
            }
            return View(bank);
        }

        public IActionResult Edit(int id)
        {
            var bank = _unitOfWork.Bank.Get(u => u.Id == id);
            if (bank == null) return NotFound();
            return View(bank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Bank bank)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Bank.Update(bank);
                _unitOfWork.Save();
                TempData["success"] = "Bank updated successfully.";
                return RedirectToAction("Index");
            }
            return View(bank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(int id)
        {
            var bank = _unitOfWork.Bank.Get(u => u.Id == id, tracked: true);
            if (bank == null) return NotFound();

            bank.IsActive = !bank.IsActive;
            _unitOfWork.Save();
            TempData["success"] = bank.IsActive ? "Bank activated." : "Bank deactivated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var bank = _unitOfWork.Bank.Get(u => u.Id == id);
            if (bank == null) return NotFound();

            var hasDisbursements = _unitOfWork.LoanDisbursement.GetAll(d => d.BankId == id).Any();
            if (hasDisbursements)
            {
                TempData["error"] = "Cannot delete a bank that has existing disbursements. Deactivate it instead.";
                return RedirectToAction("Index");
            }

            _unitOfWork.Bank.Remove(bank);
            _unitOfWork.Save();
            TempData["success"] = "Bank deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
