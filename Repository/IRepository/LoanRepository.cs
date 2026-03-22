using LoanApp.Data;
using LoanApp.Models;

namespace LoanApp.Repository.IRepository
{
    public class LoanRepository : Repository<Loan>, ILoanRepository
    {
        private readonly ApplicationDbContext _db;

        public LoanRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Loan obj)
        {
            _db.Loans.Update(obj);
        }
    }
}
