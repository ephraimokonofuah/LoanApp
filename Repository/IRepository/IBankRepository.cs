using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface IBankRepository : IRepository<Bank>
    {
        void Update(Bank obj);
    }
}
