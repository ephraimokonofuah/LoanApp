using System.Linq.Expressions;
using LoanApp.Data;
using LoanApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Repository.IRepository
{
    public class LoanApplicationRepository : Repository<LoanApplication>, ILoanApplicationRepository
    {
        private readonly ApplicationDbContext _db;

        public LoanApplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(LoanApplication obj)
        {
            _db.LoanApplications.Update(obj);
        }

        public new async Task<bool> AnyAsync(Expression<Func<LoanApplication, bool>> filter)
        {
            return await _db.LoanApplications.AnyAsync(filter);
        }
        
    }
}