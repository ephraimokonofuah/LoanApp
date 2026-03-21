using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface IEligibilityCheckRepository : IRepository<EligibilityCheck>
    {
        void Update(EligibilityCheck obj);
    }
}
