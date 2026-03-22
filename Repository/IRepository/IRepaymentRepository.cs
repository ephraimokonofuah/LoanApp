using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface IRepaymentRepository : IRepository<Repayment>
    {
        void Update(Repayment obj);
    }
}
