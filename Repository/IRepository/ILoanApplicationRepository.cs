using System.Linq.Expressions;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public interface ILoanApplicationRepository : IRepository<LoanApplication>
    {
        void Update(LoanApplication obj);
        new Task<bool> AnyAsync(Expression<Func<LoanApplication, bool>> filter);
        
    }
}