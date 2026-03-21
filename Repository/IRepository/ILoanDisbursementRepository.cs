using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface ILoanDisbursementRepository : IRepository<LoanDisbursement>
    {
        void Update(LoanDisbursement obj);
    }
}
