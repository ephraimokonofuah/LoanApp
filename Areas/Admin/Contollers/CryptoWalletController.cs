using LoanApp.Models;
using LoanApp.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "GlobalAdmin")]
    public class CryptoWalletController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CryptoWalletController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var wallets = _unitOfWork.CryptoWallet.GetAll().OrderBy(w => w.WalletType).ToList();
            return View(wallets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CryptoWallet wallet)
        {
            if (wallet.WalletType != PaymentMethodType.USDT && wallet.WalletType != PaymentMethodType.Bitcoin)
            {
                ModelState.AddModelError("WalletType", "Only USDT or Bitcoin wallet types are allowed.");
            }

            if (ModelState.IsValid)
            {
                wallet.CreatedAt = DateTime.UtcNow;
                _unitOfWork.CryptoWallet.Add(wallet);
                _unitOfWork.Save();
                TempData["success"] = "Crypto wallet added successfully.";
                return RedirectToAction("Index");
            }
            return View(wallet);
        }

        public IActionResult Edit(int id)
        {
            var wallet = _unitOfWork.CryptoWallet.Get(w => w.Id == id);
            if (wallet == null) return NotFound();
            return View(wallet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CryptoWallet wallet)
        {
            if (wallet.WalletType != PaymentMethodType.USDT && wallet.WalletType != PaymentMethodType.Bitcoin)
            {
                ModelState.AddModelError("WalletType", "Only USDT or Bitcoin wallet types are allowed.");
            }

            if (ModelState.IsValid)
            {
                var existing = _unitOfWork.CryptoWallet.Get(w => w.Id == wallet.Id, tracked: true);
                if (existing == null) return NotFound();

                existing.WalletType = wallet.WalletType;
                existing.WalletAddress = wallet.WalletAddress;
                existing.Network = wallet.Network;
                existing.Label = wallet.Label;
                existing.IsActive = wallet.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.CryptoWallet.Update(existing);
                _unitOfWork.Save();
                TempData["success"] = "Crypto wallet updated successfully.";
                return RedirectToAction("Index");
            }
            return View(wallet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var wallet = _unitOfWork.CryptoWallet.Get(w => w.Id == id);
            if (wallet == null) return NotFound();

            _unitOfWork.CryptoWallet.Remove(wallet);
            _unitOfWork.Save();
            TempData["success"] = "Crypto wallet deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
