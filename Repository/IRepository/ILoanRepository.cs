using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface ILoanRepository : IRepository<Loan>
    {
        void Update(Loan obj);
    }
}
