using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface ILoanTypeRepository : IRepository<LoanType>
    {
        void Update(LoanType obj);
    }
}
