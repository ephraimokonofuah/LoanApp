using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class CryptoWalletRepository : Repository<CryptoWallet>, ICryptoWalletRepository
    {
        private readonly ApplicationDbContext _db;

        public CryptoWalletRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CryptoWallet obj)
        {
            _db.CryptoWallets.Update(obj);
        }
    }
}
