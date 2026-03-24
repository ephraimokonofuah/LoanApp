using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface ICryptoWalletRepository : IRepository<CryptoWallet>
    {
        void Update(CryptoWallet obj);
    }
}
